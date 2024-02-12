using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Models.Weather;
using WebApi.Settings;
using SharedModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApi.Controllers;

/// <summary>
/// Погода
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;
    private readonly IRequestClient<WeatherRequest> _client;
    private readonly IOptions<WeatherApiSettings> _options;
    private readonly IPublishEndpoint _publishEndpoint;

    public WeatherController(ILogger<WeatherController> logger,
        IRequestClient<WeatherRequest> client,  IOptions<WeatherApiSettings> options,IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _client = client;
        _options = options;
        _publishEndpoint = publishEndpoint;
    }

    /// <summary>
    /// Получить прогноз погоды 
    /// </summary>
    ///<param name="Spot">название города или координаты спота<example>London</example></param>
    ///<param name="Days">на сколько дней прогноз<example>3</example></param>
    /// <returns></returns>
    [HttpPost("get_weather_forecast")]
    public async Task<ActionResult> GetForecast(WeatherRequestDto request)
    {
        var Key = _options.Value.WeatherApiKey;
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        await  _publishEndpoint.Publish<WeatherRequest>(new
            {  userId, Aqi = "no", Alerts = "no", request.Days, Key, request.Spot });
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("get_weather_allspots_forecast")]
    public async Task<ActionResult> GetAllSpotsForecast( WeatherSpotsRequestDto request)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //request.UserId = userId.ToString();
        request.Key = _options.Value.WeatherApiKey; //"b51fa035a98a499a82b144255222212";
        await  _publishEndpoint.Publish<WeatherSpotsRequest>(new
            { userId, request.Aqi, request.Alerts, request.Days, request.Key, request.Spots });
        return Ok();
    }
}
