using Core.Abstractions.Repositories;
using Core.Domain;
using Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IO;
using WebApi.Mappers;
using WebApi.Models;
using WebApi.Utils;


namespace WebApi.Controllers;

/// <summary>
/// Фото мест активности
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]

public class SpotPhotoController : ControllerBase
{
    private readonly IRepository<SpotPhoto> _spotPhotoRepository;
    private readonly ILogger<SpotPhotoController> _logger;
    private readonly IOptions<WindSharingSettings> _options;
    private readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("ru-ru");

    public SpotPhotoController(IRepository<SpotPhoto> spotPhotoRepository, ILogger<SpotPhotoController> logger,
        IOptions<WindSharingSettings> options)
    {
        _spotPhotoRepository = spotPhotoRepository;
        _logger = logger;
        _options = options;
    }

    /// <summary>
    /// Получить данные фотографий по фильтру
    /// </summary>
    /// <param name="id">Id spotPhoto, например, <example>30BE0198-7A29-4EE6-8BAB-B702F7A357E2</example></param>
    /// <param name="fileName">имя файла, например, <example>desk.wep</example></param>
    /// <param name="idSpot">idSpot идентификатор Spot, например, <example>4BCC4F3B-DB71-4442-A65B-556E2513700D</example></param>
    /// <param name="dateFrom">нижний предел даты создания файла, например, <example>01.01.2023</example></param>
    /// <param name="dateTo">верхний предел даты создания файла, например, <example>28.02.2023</example></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<SpotPhoto>>> GetSpotPhotoWhereAsync(Guid? id,
        string? fileName, Guid? idSpot, string? dateFrom, string? dateTo)
    {
        var query = _spotPhotoRepository.GetAsQueryable();
        if (id != null) query = query.Where(x => x.Id == id);
        if (!string.IsNullOrEmpty(fileName)) query = query.Where(x => x.FileName.Contains(fileName));
        if (idSpot != null) query = query.Where(x => x.SpotId == idSpot);
        if (DateTime.TryParse(dateFrom, _culture, DateTimeStyles.None, out var date1))
        {
            query = query.Where(x => x.CreateDataTime >= date1);
        }
        if (DateTime.TryParse(dateTo, _culture, DateTimeStyles.None, out var date2))
        {
            query = query.Where(x => x.CreateDataTime <= date2);
        }

        var spotPhotos = await query.ToListAsync();

        var response = spotPhotos.Select(x => new SpotPhotoResponse(x)).ToList();
        return Ok(response);

    }

    /// <summary>
    /// Получить запись spotPhoto  по id без фотографии
    /// </summary>
    /// <param name="id">Id spotPhoto, например, <example>30BE0198-7A29-4EE6-8BAB-B702F7A357E2</example></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetSpotPhotoAsync(Guid id)
    {
        var spotPhoto = await _spotPhotoRepository.GetByIdAsync(id);
        if (spotPhoto == null) return NotFound();
        return Ok(new SpotPhotoResponse(spotPhoto));
    }

    /// <summary>
    /// Создать spotPhoto
    /// </summary>
    /// <param name="request"></param>
    /// <param name="dataFile"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<SpotPhotoResponse>> CreateSpotPhotoAsync(CreateOrEditSpotPhotoRequest request, IFormFile? dataFile)
    {
        var spotPhoto = SpotPhotoMapper.MapFromModel(request);

        await _spotPhotoRepository.CreateAsync(spotPhoto);

        if (dataFile != null)
        {
            string fullFileName = FileHelper.FileNameWithExt(spotPhoto.FileName, _options.Value.DirFullPhoto!, spotPhoto.Id.ToString());
            // сохраняем файл в папку 
            await using var fileStream = new FileStream(fullFileName, FileMode.Create);
            await dataFile.CopyToAsync(fileStream);
        }

        _logger.LogInformation($"Добавлена новая запись фотографии места активности '{request.FileName}' (ID = {spotPhoto.Id})");

        return CreatedAtAction(nameof(GetSpotPhotoAsync), new { id = spotPhoto.Id }, null);
    }

    /// <summary>
    /// Обновить spotPhoto по id без файла (нельзя менять расширение файла)
    /// </summary>
    /// <param name="id">Id spotPhoto, например, <example>30BE0198-7A29-4EE6-8BAB-B702F7A357E2</example></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditSpotPhotoAsync(Guid id, CreateOrEditSpotPhotoRequest request)
    {
        var spotPhoto = await _spotPhotoRepository.GetByIdAsync(id);

        if (spotPhoto == null)
            return NotFound();

        SpotPhotoMapper.MapFromModel(request, spotPhoto);

        await _spotPhotoRepository.UpdateAsync(spotPhoto);
        string log = $"Изменено SpotPhoto (ID = {spotPhoto.Id})";
        _logger.LogInformation(log);

        return NoContent();
    }

    /// <summary>
    /// Удалить SpotPhoto по id
    /// </summary>
    /// <param name="id">Id SpotPhoto, например, <example>30BE0198-7A29-4EE6-8BAB-B702F7A357E2</example></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSpotPhotoAsync(Guid id)
    {
        var spotPhoto = await _spotPhotoRepository.GetByIdAsync(id);

        if (spotPhoto == null)
            return NotFound();

        string fullFileName = FileHelper.FileNameWithExt(spotPhoto.FileName, _options.Value.DirFullPhoto!, spotPhoto.Id.ToString());
        if (!System.IO.File.Exists(fullFileName))
        {
            _logger.LogInformation($"Не найден файл {fullFileName} для удаления в файловом хранилище SpotPhoto при удалении записи (ID = {spotPhoto.Id})");
        }
        else
        {
            System.IO.File.Delete(fullFileName);
        }
        string log = $"Удалено SpotPhoto '{spotPhoto.FileName}' (ID = {spotPhoto.Id})";

        await _spotPhotoRepository.DeleteAsync(id);

        _logger.LogInformation(log);

        return NoContent();
    }

    /// <summary>
    /// Получить файл фотографии spotPhoto по Id или имени файла (при указании двух параметров поиск по Id сущности)
    /// </summary>
    /// <param name="id">Id файла, например, <example>30BE0198-7A29-4EE6-8BAB-B702F7A357E2</example></param>
    /// <param name="filename">имя файла, например, <example>30BE0198-7A29-4EE6-8BAB-B702F7A357E2.jpg</example></param>
    /// <returns></returns>
    [HttpGet("photo")]
    [AllowAnonymous]
    public async Task<ActionResult> GetSpotPhotoFileAsync(Guid? id, string? filename)
    {
        string fullFileName;
        if (id != null)
        {
            var spotPhoto = await _spotPhotoRepository.GetByIdAsync(id.Value);
            if (spotPhoto == null) return NotFound();
            filename = spotPhoto.FileName;
            fullFileName = FileHelper.FileNameWithExt(spotPhoto.FileName, _options.Value.DirFullPhoto!,
                spotPhoto.Id.ToString());
        }
        else if (!string.IsNullOrWhiteSpace(filename))
        {
            fullFileName = Path.Combine(_options.Value.DirFullPhoto ?? "", filename);
        }
        else
        {
            return BadRequest();
        }

        if (!System.IO.File.Exists(fullFileName))
        {
            return NotFound();
        }

        return new FileStreamResult(new FileStream(fullFileName, FileMode.Open, FileAccess.Read), FileHelper.GetMimeType(filename!));
    }

    /// <summary>
    /// Обновить файл в записи SpotPhoto по id
    /// </summary>
    /// <param name="id">Id SpotPhoto, например, <example>30BE0198-7A29-4EE6-8BAB-B702F7A357E2</example></param>
    /// <param name="data">файловый поток</param>
    /// <returns></returns>
    [HttpPut("photo/{id:guid}")]
    public async Task<IActionResult> EditSpotPhotoFileAsync(Guid id, IFormFile data)
    {
        var spotPhoto = await _spotPhotoRepository.GetByIdAsync(id);

        if (spotPhoto == null)
        {
            //return NotFound();
            spotPhoto = new SpotPhoto
            {
                Id = Guid.NewGuid(),
                CreateDataTime = DateTime.Now,
                FileName = data.FileName,
                SpotId = id,
            };
            await _spotPhotoRepository.CreateAsync(spotPhoto);
        }
        string fullFileName = FileHelper.FileNameWithExt(spotPhoto.FileName, _options.Value.DirFullPhoto!, spotPhoto.Id.ToString());
        if (System.IO.File.Exists(fullFileName))
        {
            System.IO.File.Delete(fullFileName);
        }
        else
        {
            _logger.LogInformation($"Не найден файл {fullFileName} для удаления в файловом хранилище SpotPhoto при редактировании записи (ID = {spotPhoto.Id})");
        }

        await using (var fileStream = new FileStream(fullFileName, FileMode.Create))
        {
            await data.CopyToAsync(fileStream);
        }

        spotPhoto.FileName = data.FileName;
        await _spotPhotoRepository.UpdateAsync(spotPhoto);
        string log = $"Обновлен файл в SpotPhoto '{spotPhoto.FileName}' (ID = {spotPhoto.Id})";
        _logger.LogInformation(log);

        return NoContent();
    }
}