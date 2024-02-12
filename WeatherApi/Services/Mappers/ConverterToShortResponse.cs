using System;
using System.Collections.Generic;
using WeatherApi.Models;

namespace WeatherApi.Mappers
{
    public class ConverterToShortResponse : IConverterToShortResponse
    {
        public WeatherResponseShortDto ConvertToShortResponse(WeatherResponseDto item)
        {
            ForecastShort fs = new ForecastShort();
            var fds_list = new List<ForecastdayShort>();

            foreach (Forecastday fd in item.forecast.forecastday)
            {
                ForecastdayShort fds = new ForecastdayShort
                {
                    date = fd.date,
                    day = new DayShort
                    {
                        maxtemp_c = fd.day.maxtemp_c,
                        mintemp_c = fd.day.mintemp_c,
                        maxwind_kph = fd.day.maxwind_kph,
                        avghumidity = fd.day.avghumidity,
                        avgvis_km = fd.day.avgvis_km,
                    }
                };
                fds_list.Add(fds);
            }
            fs.forecastday = fds_list;

            CurrentShort cs = new CurrentShort
            {
                temp_c = item.current.temp_c,
                is_day = item.current.is_day,
                wind_dir = item.current.wind_dir,
                wind_kph = item.current.wind_kph,
                gust_kph = item.current.gust_kph,
                uv = item.current.uv,
                condition = item.current.condition
            };

            LocationShort ls = new LocationShort
            {
                name = item.location.name,
                region = item.location.region,
                country = item.location.country,
                lon = item.location.lon,
                lat = item.location.lat
            };

            WeatherResponseShortDto wrs = new WeatherResponseShortDto
            {
                location = ls,
                current = cs,
                forecast = fs
            };
            return wrs;
        }
    }
}