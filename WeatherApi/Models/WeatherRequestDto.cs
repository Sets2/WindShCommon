namespace WeatherApi.Models{
    public class WeatherRequestDto
    {
        public string? Key { get; set; }
        public string? Q { get; set; }
        public string? Days { get; set; }
        public string? Aqi { get; set; }
        public string? Alerts { get; set; }
        public string? Lang { get; set; }
    }
}