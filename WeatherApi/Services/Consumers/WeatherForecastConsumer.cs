using MassTransit;
using WeatherApi.Models;
using WeatherApi.Services.Weather;
using SharedModel;
using WeatherApi.RedisCache;
using Microsoft.AspNetCore.SignalR;
using WeatherApi.Services.Hub;
using WeatherApi.Mappers;

namespace WeatherApi.Services.Consumers {
    public class WeatherForecastConsumer : 
        IConsumer<WeatherRequest>
    {
        private readonly  IGivingWeatherForecast _givingWeatherForecast;
        private readonly IWeatherCacheService _weatherCacheService;
        private readonly IHubContext<WeatherHub, IWeatherHubClient> _hubContext;
        private readonly IConverterToShortResponse _converterToShortResponse;
        public WeatherForecastConsumer(IGivingWeatherForecast givingWeatherForecast,IWeatherCacheService weatherCacheService,
         IHubContext<WeatherHub, IWeatherHubClient> hubContext,IConverterToShortResponse converterToShortResponse)
        {
            _givingWeatherForecast = givingWeatherForecast;
            _weatherCacheService = weatherCacheService;
            _hubContext = hubContext;
            _converterToShortResponse = converterToShortResponse;
        }
        public async Task Consume(ConsumeContext<WeatherRequest> context)
        {
            var cacheData = _weatherCacheService.GetData<WeatherResponseDto> ("weatherResponse" + context.Message.Spot);
            if (cacheData != null) 
            {
                
            }
            else
            {
                WeatherRequestDto request = new WeatherRequestDto{ Aqi = context.Message.Aqi, Alerts = context.Message.Alerts,
                Key = context.Message.Key, Q = context.Message.Spot, Days = context.Message.Days} ;
                var response = await _givingWeatherForecast.GiveWeatherByCoordinates(request);
                if (response == null)
                    throw new InvalidOperationException("Weather response is unavailable");
                var expirationTime = DateTimeOffset.Now.AddMinutes(1440.0);
                cacheData = response;
                _weatherCacheService.SetData<WeatherResponseDto>("weatherResponse" + context.Message.Spot,cacheData, expirationTime);
                
            }
            WeatherResponseShortDto result =  _converterToShortResponse.ConvertToShortResponse(cacheData);
            var clientGroup = _hubContext.Clients.Group(context.Message.UserId.ToString());
            await clientGroup.RecieveWeather(result);

        }
    }
}