using WeatherApi.Models;

namespace  WeatherApi.Services.Weather
{
    public interface IGivingWeatherForecast
    {
        Task<WeatherResponseDto?> GiveWeatherByCoordinates(WeatherRequestDto dto);
    }
}