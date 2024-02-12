using Core.Abstractions.Repositories;
using Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Controllers;

/// <summary>
/// Вид активности
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ActivityController : ControllerBase
{
    private IRepository<Activity> _activityRepository;
    private ILogger<ActivityController> _logger;

    public ActivityController(IRepository<Activity> activityRepository, ILogger<ActivityController> logger)
    {
        _activityRepository = activityRepository;
        _logger = logger;
    }

    /// <summary>
    /// Получить данные всех активностей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ActivityResponse>>> GetActivitiesAllAsync()
    {
        var activities = await _activityRepository.GetAllAsync();
        var response = activities.Select(x => new ActivityResponse(x)).ToList();
        return Ok(response);
    }

    /// <summary>
    /// Получить активность по id
    /// </summary>
    /// <param name="id">Id активности, например, <example>d75ca686-2e5f-4dfd-b4ad-3a5528b7fefe</example></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ActivityResponse>> GetActivityAsync(Guid id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        if (activity == null) return NotFound();
        var response = new ActivityResponse(activity);
        return Ok(response);
    }

    /// <summary>
    /// Создать активность
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ActivityResponse>> CreateActivityAsync(CreateOrEditActivityRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var activity = ActivityMapper.MapFromModel(request);

        await _activityRepository.CreateAsync(activity);

        _logger.LogInformation($"Добавлена новая активность '{request.Name}' (ID = {activity.Id})");

        return CreatedAtAction(nameof(GetActivityAsync), new { id = activity.Id }, null);
    }

    /// <summary>
    /// Обновить активность по id
    /// </summary>
    /// <param name="id">Id активности, например, <example>db0208a4-2609-4f07-8c50-8fd9e064d9a4</example></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditActivitiesAsync(Guid id, CreateOrEditActivityRequest request)
    {
        var activity = await _activityRepository.GetByIdAsync(id);

        if (activity == null)
            return NotFound();

        string log = $"Изменена активность с '{activity.Name}' на '{request.Name}' (ID = {activity.Id})";

        ActivityMapper.MapFromModel(request, activity);

        await _activityRepository.UpdateAsync(activity);

        _logger.LogInformation(log);

        return NoContent();
    }

    /// <summary>
    /// Удалить активность по id
    /// </summary>
    /// <param name="id">Id активности, например, <example>db0208a4-2609-4f07-8c50-8fd9e064d9a4</example></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteActivityAsync(Guid id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);

        if (activity == null)
            return NotFound();

        string log = $"Удалена активность '{activity.Name}' (ID = {activity.Id})";

        await _activityRepository.DeleteAsync(id);

        _logger.LogInformation(log);

        return NoContent();
    }
}