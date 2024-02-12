namespace WebApi.Models.Weather{
    public class WeatherRequestDto
    {
        public string Spot { get; set; }       
        public int Days { get; set; }
    }
}