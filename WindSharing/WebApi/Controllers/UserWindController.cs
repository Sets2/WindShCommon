using Core.Abstractions.Repositories;
using Core.Domain;
using Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using WebApi.Auth;
using WebApi.Mappers;
using WebApi.Models;
using WebApi.Utils;

namespace WebApi.Controllers;

/// <summary>
/// Пользователи
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserWindController : ControllerBase
{
    private readonly IRepository<UserWind> _userWindRepository;
    private readonly ILogger<AuthController> _logger;
    private readonly IOptions<WindSharingSettings> _options;

    public UserWindController(IRepository<UserWind> userWindRepository, ILogger<AuthController> logger,
        IOptions<WindSharingSettings> options)
    {
        _userWindRepository = userWindRepository;
        _logger = logger;
        _options = options;
    }

    /// <summary>
    /// Получить данные всех пользователей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<UserWindResponse>> GetUserWindsAll()
    {
        var userWinds = await _userWindRepository.GetAllAsync();
        var response = userWinds
                .Select(x => new UserWindResponse(x)).OrderBy(x=>x.Fio).ToList();

        return Ok(response);
    }

    /// <summary>
    /// Получить пользователя по id
    /// </summary>
    /// <param name="id">Id пользователя, например, <example>41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0</example></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserWindResponse>> GetUserWindAsync(Guid id)
    {
        var userWind = await _userWindRepository.GetByIdAsync(id);
        if (userWind == null) return NotFound();
        var response = new UserWindResponse(userWind);
        return Ok(response);
    }

    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <param name="dataFile"></param>
    /// <returns></returns>
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<UserWindResponse>> CreateUserWindAsync(CreateOrEditUserWindRequest request,
        IFormFile? dataFile)
    {
        var userWinddouble = await _userWindRepository.
            GetFirstWhere(x => x.NormalizedUserName == Strings.UCase(request.UserName));
        if (userWinddouble != null) return BadRequest("Пользователь с таким UserName уже существует");
        var userWind = UserWindMapper.MapFromModel(request);
        userWind.IsActive = true;

        await _userWindRepository.CreateAsync(userWind);
        _logger.LogInformation($"Добавлен пользователь '{userWind.UserName}' (ID = {userWind.Id})");

        if (dataFile != null && !string.IsNullOrWhiteSpace(userWind.FotoFileName))
        {
            string fullFileName = FileHelper.FileNameWithExt(userWind.FotoFileName, _options.Value.DirFullPhoto!, userWind.Id.ToString());
            // сохраняем файл в папку 
            await using var fileStream = new FileStream(fullFileName, FileMode.Create);
            await dataFile.CopyToAsync(fileStream);
            _logger.LogInformation($"Добавлена фотография пользователя '{userWind.UserName} {userWind.FotoFileName}' (ID = {userWind.Id})");
        }

        return CreatedAtAction(nameof(GetUserWindAsync), new { id = userWind.Id }, null);
    }

    /// <summary>
    /// Обновить пользовтаеля по id без фотографии (нельзя менять расширение файла фотографии)
    /// </summary>
    /// <param name="id">Id пользователя, например, <example>41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0</example></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = OwnerRequirement.Name)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditUserWindAsync(Guid id, CreateOrEditUserWindRequest request)
    {
        var userWind = await _userWindRepository.GetByIdAsync(id);
        if (userWind == null) return NotFound();
        var userWinddouble =
            await _userWindRepository.GetFirstWhere(x => x.NormalizedUserName == Strings.UCase(request.UserName));
        if ((userWinddouble != null) && (userWinddouble.Id != userWind.Id))
            return BadRequest("Пользователь с таким UserName уже существует");

        UserWindMapper.MapFromModel(request, userWind);

        await _userWindRepository.UpdateAsync(userWind);
        _logger.LogInformation($"Обновлен пользователь '{userWind.UserName}' (ID = {userWind.Id})");

        return NoContent();
    }

    /// <summary>
    /// Удалить пользователя по id
    /// </summary>
    /// <param name="id">Id пользователя, например, <example>41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0</example></param>
    /// <returns></returns>

    [Authorize(Policy = AdminOrOwnerRequirement.Name)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUserWindAsync(Guid id)
    {
        var userWind = await _userWindRepository.GetByIdAsync(id);
        if (userWind == null) return NotFound();
        var userName = userWind.UserName;
        if (!string.IsNullOrWhiteSpace(userWind.FotoFileName))
        {
            string fullFileName = FileHelper.FileNameWithExt(userWind.FotoFileName, _options.Value.DirFullPhoto!, userWind.Id.ToString());
            if (!System.IO.File.Exists(fullFileName))
            {
                _logger.LogInformation($"Не найден файл {fullFileName} для удаления в файловом хранилище SpotPhoto при удалении записи (ID = {id})");
            }
            else
            {
                System.IO.File.Delete(fullFileName);
            }
        }
        await _userWindRepository.DeleteAsync(id);
        _logger.LogInformation($"Удален пользователь '{userName}' (ID = {id})");

        return NoContent();
    }
    /// <summary>
    /// Получить файл фотографии пользователя по Id или имени файла (при указании двух параметров поиск по Id сущности)
    /// </summary>
    /// <param name="id">Id пользователя, например, <example>41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0</example></param>
    /// <param name="filename">имя файла, например, <example>41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0.jpg</example></param>
    /// <returns></returns>
    [HttpGet("photo")]
    public async Task<ActionResult> GetUserWindPhotoFileAsync(Guid? id, string? filename)
    {
        string fullFileName;
        if (id != null)
        {
            var userWind = await _userWindRepository.GetByIdAsync(id.Value);
            if (userWind == null || string.IsNullOrWhiteSpace(userWind.FotoFileName))
            {
                return NotFound();
            }
            filename = userWind.FotoFileName;
            fullFileName = FileHelper.FileNameWithExt(filename, _options.Value.DirFullPhoto!,
                userWind.Id.ToString());
        }
        else if (!string.IsNullOrWhiteSpace(filename))
        {
            fullFileName = @$"{_options.Value.DirFullPhoto}/{filename}";
        }
        else
        {
            return BadRequest();
        }

        if (!System.IO.File.Exists(fullFileName))
        {
            return NotFound();
        }

        return new FileStreamResult(new FileStream(fullFileName, FileMode.Open), FileHelper.GetMimeType(filename));
    }

    /// <summary>
    /// Обновить файл пользователя по id
    /// </summary>
    /// <param name="id">Id пользователя, например, <example>41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0</example></param>
    /// <param name="data">файловый поток</param>
    /// <returns></returns>
    //[Authorize(Policy = AdminOrOwnerRequirement.Name)]
    [HttpPut("photo/{id:guid}")]
    public async Task<IActionResult> EditUserWindPhotoFileAsync(Guid id, [FromForm] IFormFile data)
    {
        var userWind = await _userWindRepository.GetByIdAsync(id);
        
        if (userWind == null || string.IsNullOrWhiteSpace(data.FileName))
            return NotFound();
        if (!string.IsNullOrWhiteSpace(userWind.FotoFileName))
        {
            string oldFullFileName = FileHelper.FileNameWithExt(userWind.FotoFileName, _options.Value.DirFullPhoto!, userWind.Id.ToString());
            if (!System.IO.File.Exists(oldFullFileName))
            {
                _logger.LogInformation($"Не найден файл {oldFullFileName} для удаления в файловом хранилище при обновлении записи (ID = {userWind.Id})");
            }
            else
            {
                System.IO.File.Delete(oldFullFileName);
            }
        }
        string newPhotoFileName = $"{userWind.Id}{Path.GetExtension(data.FileName)}";
        string fullFileName = FileHelper.FileNameWithExt(newPhotoFileName, _options.Value.DirFullPhoto!, userWind.Id.ToString());
        
        await using (var fileStream = new FileStream(fullFileName, FileMode.Create))
        {
            await data.CopyToAsync(fileStream);
        }

        userWind.FotoFileName = newPhotoFileName;
        await _userWindRepository.UpdateAsync(userWind);
        string log = $"Обновлен файл в хранилище '{userWind.FotoFileName}' (userWind ID = {userWind.Id})";
        _logger.LogInformation(log);
        return NoContent();
    }

    /// <summary>
    /// Отключение/включение пользователя
    /// </summary>
    /// <param name="id">Id пользователя, например, <example>41e6095e-4efb-4f0e-b06e-c8bfa54e1ab0</example></param>
    /// <returns></returns>
    [HttpPut("toggle/{id:guid}")]
    public async Task<IActionResult> ToggleUserWindActiveAsync(Guid id)
    {
        var userWind = await _userWindRepository.GetByIdAsync(id);
        if (userWind == null) return NotFound();
        userWind.IsActive = !userWind.IsActive;
        await _userWindRepository.UpdateAsync(userWind);
        _logger.LogInformation($"Обновлен пользователь '{userWind.UserName}' (ID = {userWind.Id})");

        return NoContent();
    }
}

