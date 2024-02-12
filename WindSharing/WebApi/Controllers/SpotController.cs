using Core.Abstractions.Repositories;
using Core.Domain;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Controllers;

/// <summary>
/// Спот
/// </summary>
//[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SpotController : ControllerBase
{
    //private IRepositorySpot<Spot> _spotRepoSpot;
    private IRepository<Spot> _spotRepository;
    private IRepository<UserWind> _userWindRepository;
    private IRepository<UserSpot> _userSpotRepository;
    private ILogger<AuthController> _logger;

    public SpotController(IRepository<Spot> spotRepository,
                            ILogger<AuthController> logger,
                            //IRepositorySpot<Spot> spotRepoSpot,
                            IRepository<UserWind> userWindRepository,
                            IRepository<UserSpot> userSpotRepository
                            )
    {
        _spotRepository = spotRepository;
        _logger = logger;
        //_spotRepoSpot = spotRepoSpot;
        _userWindRepository = userWindRepository;
        _userSpotRepository = userSpotRepository;
    }

    /// <summary>
    /// Получить данные всех доступных спотов
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<SpotResponse>> GetSpotsAll()
    {
        var spots = await _spotRepository.GetAllAsync();
        var response = spots
                .Select(x => new SpotResponse(x)).ToList();

        return Ok(response);
    }

    [HttpGet("{take:int}/{skip:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<SpotResponse>> GetSpotsNotes(int take = 50, int skip = 0)
    {
        var spots = await _spotRepository.GetAllPaginateAsync(take, skip);
        var response = spots.Select(x => new SpotResponse(x)).ToList();
        return Ok(response);
    }


    /// <summary>
    /// Получить спот по id
    /// </summary>
    /// <param name="id">Id спота, например, <example>f91dbebe-04a8-46ba-877e-62ef624a77f4</example></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<SpotByIdResponse>> GetSpotAsync(Guid id)
    {
        var spot = await _spotRepository.GetByIdAsync(id);
        if (spot == null) return NotFound();
        var response = new SpotByIdResponse(spot);
        return Ok(response);
    }

    /// <summary>
    /// Создать новый спот
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<SpotResponse>> CreateSpotAsync([FromBody] CreateOrEditSpotRequest request)
    {
        var spot = SpotMapper.MapFromModel(request);
        spot.IsActive = true;
        await _spotRepository.CreateAsync(spot);
        _logger.LogInformation($"Создан новый спот Name: {spot.Name}");
        return CreatedAtAction(nameof(GetSpotAsync), new { id = spot.Id }, spot);
    }

    /// <summary>
    /// Обновить спот по id
    /// </summary>
    /// <param name="id">Id спота, например, <example>f91dbebe-04a8-46ba-877e-62ef624a77f4</example></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditSpotAsync(Guid id, [FromBody] CreateOrEditSpotRequest request)
    {
        var spot = await _spotRepository.GetByIdAsync(id);

        if (spot == null)
            return NotFound();

        SpotMapper.MapFromModel(request, spot);

        await _spotRepository.UpdateAsync(spot);
        _logger.LogInformation($"Редактирование спота Name: {spot.Name}");
        return NoContent();
    }

    /// <summary>
    /// Удалить спот по id
    /// </summary>
    /// <param name="id">Id спота, например, <example>f91dbebe-04a8-46ba-877e-62ef624a77f4</example></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSpotAsync(Guid id)
    {
        var spot = await _spotRepository.GetByIdAsync(id);

        if (spot == null)
            return NotFound();

        await _spotRepository.DeleteAsync(id);
        _logger.LogInformation($"Удален спот: {spot.Name}");
        return NoContent();
    }

    /// <summary>
    /// Отметить себя на споте
    /// </summary>
    /// <param name="spot_id">Id спота, например, <example>f91dbebe-04a8-46ba-877e-62ef624a77f4</example></param>
    /// <param name="user_id">Id пользователя, например, <example>f91dbebe-04a8-46ba-877e-62ef624a77f4</example></param>
    /// <param name="comment">Комментарий пользователя о споте, например, <example>Лучше не куда</example></param>
    /// <returns></returns>
    [HttpPost("iamhere")]
    public async Task<IActionResult> IamHereSpotAsync([FromBody] UserSpotRequest item)
    {
        var spot = await _spotRepository.GetByIdAsync(item.SpotId);

        if (spot == null)
            return NotFound();

        var user = await _userWindRepository.GetByIdAsync(item.UserId);

        if (user == null)
            return NotFound();

        var userSpot =
            await _userSpotRepository.GetFirstWhere(x => x.SpotId == item.SpotId && x.UserWindId == item.UserId);
        if (userSpot != null)
            return BadRequest($"На Spot с Id={item.SpotId} уже сществует запись для пользователя с Id={item.UserId}");

        await _userSpotRepository.CreateAsync(new UserSpot
        {
            Comment = item.Comment,
            CreateDataTime = DateTime.Now,
            SpotId = item.SpotId,
            UserWindId = item.UserId
        });

        return NoContent();
    }
}
