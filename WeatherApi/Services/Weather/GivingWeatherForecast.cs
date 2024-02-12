using System.Text.Json;
using WeatherApi.Models;

namespace WeatherApi.Services.Weather
{
    public class GivingWeatherForecast
        : IGivingWeatherForecast
    {
        private readonly HttpClient _httpClient;

        public GivingWeatherForecast(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        

        public async Task<WeatherResponseDto?> GiveWeatherByCoordinates(WeatherRequestDto dto)
        {
            string url = string.Format("?key={0}&q={1}&days={2}&aqi={3}&alerts={4}&lang={4}", dto.Key,dto.Q,dto.Days,dto.Aqi,dto.Alerts,dto.Lang);
            var response = await _httpClient.PostAsJsonAsync(url,"");
            var p = await response.Content.ReadFromJsonAsync<object>();
            string serializer = JsonSerializer.Serialize(p);
            WeatherResponseDto? result = JsonSerializer.Deserialize<WeatherResponseDto>(serializer);
            return result;
        }
    }
}