using Quartz;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Core.Domain;
using Core.Abstractions.Repositories;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedModel;
using WebApi.Settings;
using WebApi.Controllers;
using WebApi.Models.Weather;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;

namespace WebApi.Jobs;

public class WeatherRequester : IJob, IWeatherRequester
{
    private readonly IRepository<Spot> _spotRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOptions<WeatherApiSettings> _options;
    private readonly ILogger<WeatherRequester> _logger;
    private const int _sizePackage =100;
    public WeatherRequester(ILogger<WeatherRequester> logger, IRepository<Spot> spotRepository,
        IPublishEndpoint publishEndpoint, IOptions<WeatherApiSettings> options)
    {
        _spotRepository = spotRepository;
        _options = options;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";

        _logger.LogInformation(
            $"Запрос на обновление прогноза погоды по всем Spot, время {context.FireTimeUtc.ToString()}");
        Console.WriteLine($"Quartz {context.FireTimeUtc.ToString()}" );
        var spots = await _spotRepository.GetAsQueryable().Select(x => 
            string.Join(",", x.Latitude.ToString(nfi), x.Longitude.ToString(nfi))).ToListAsync(); 
        if(spots==null) return;

        var curElement = 0;
        var countElements = spots.Count;
        var curSizePackage = 0;
        WeatherSpotsRequestDto request = new();
        request.Key = _options.Value.WeatherApiKey; //"b51fa035a98a499a82b144255222212";

        while (curElement< countElements)
        {
            curSizePackage = Math.Min(countElements - curElement,_sizePackage);

            // var response = await _client.GetResponse<WeatherResponse>(new {request.Aqi, request.Alerts, request.Days, request.Key, request.Q});
            request.Spots = spots.GetRange( curElement, curSizePackage);
            await _publishEndpoint.Publish<WeatherSpotsRequest>(new
                { request.UserId, request.Aqi, request.Alerts, request.Days, request.Key, request.Spots });

            Console.WriteLine($"Запрошен пакет с {curElement} в количестве {curSizePackage}");            

            curElement +=_sizePackage;
        }
    }
}