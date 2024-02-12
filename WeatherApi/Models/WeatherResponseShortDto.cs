namespace WeatherApi.Models{
    

   
    public class CurrentShort
    {
        public double temp_c { get; set; }
        public int is_day { get; set; }
        public double wind_kph { get; set; }
        public string? wind_dir { get; set; }
        public double gust_kph { get; set; }
        public double uv { get; set; }
        public Condition? condition { get; set; }
    }

    public class ConditionShort
    {
        public string? icon { get; set; }
        public string? text { get; set; }
    }
    public class DayShort
    {
        public double maxtemp_c { get; set; }
        public double mintemp_c { get; set; }
        public double maxwind_kph { get; set; }
        public double avghumidity { get; set; }
        public double avgvis_km { get; set; }
    }

    public class ForecastShort
    {
        public List<ForecastdayShort>? forecastday { get; set; }
    }

    public class ForecastdayShort
    {
        public string? date { get; set; }
        public DayShort? day { get; set; }
    }

    public class LocationShort
    {
        public string? name { get; set; }
        public string? region { get; set; }
        public string? country { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }

    }

    public class WeatherResponseShortDto
    {
        public LocationShort? location { get; set; }
        public CurrentShort? current { get; set; }
        public ForecastShort? forecast { get; set; }
    }

}