namespace WebApi.Models.Weather{
    public class WeatherSpotsRequestDto
    {
        public Guid? UserId { get; set; } = null;
        public string? Key { get; set; } = "10";
        public List<string>? Spots { get; set; }
        public string? Days { get; set; }
        public string? Aqi { get; set; }
        public string? Alerts { get; set; }
    }
}