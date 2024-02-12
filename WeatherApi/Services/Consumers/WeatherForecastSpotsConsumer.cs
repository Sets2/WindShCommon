using MassTransit;
using WeatherApi.Models;
using WeatherApi.Services.Weather;
using SharedModel;
using WeatherApi.RedisCache;
using WeatherApi.Services.Hub;
using Microsoft.AspNetCore.SignalR;

namespace WeatherApi.Services.Consumers {
    public class WeatherForecastSpotsConsumer : 
        IConsumer<WeatherSpotsRequest>
    {
        private readonly IGivingWeatherForecast _givingWeatherForecast;
        private readonly IWeatherCacheService _weatherCacheService;
        private readonly IHubContext<WeatherHub, IWeatherHubClient> _hubContext;
        public WeatherForecastSpotsConsumer(IGivingWeatherForecast givingWeatherForecast,IWeatherCacheService weatherCacheService
        ,IHubContext<WeatherHub, IWeatherHubClient> hubContext)
        {
            _givingWeatherForecast = givingWeatherForecast;
            _weatherCacheService = weatherCacheService;
            _hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<WeatherSpotsRequest> context)
        {
            foreach( string spot in context.Message.Spots! )
            {
                var cacheData = _weatherCacheService.GetData<WeatherResponseDto> ("weatherResponse" + spot);
                if (cacheData != null) 
                {
                    
                }
                else
                {
                    WeatherRequestDto request = new WeatherRequestDto{ Aqi = context.Message.Aqi, Alerts = context.Message.Alerts,
                    Key = context.Message.Key, Q = spot, Days = context.Message.Days} ;
                    var response_entry = await _givingWeatherForecast.GiveWeatherByCoordinates(request);
                    if (response_entry != null){
                        var expirationTime = DateTimeOffset.Now.AddMinutes(1440.0);
                        _weatherCacheService.SetData<WeatherResponseDto>("weatherResponse"+ spot,response_entry, expirationTime);
                    }
                }
            
            }
        }
    }
}