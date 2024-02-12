using System;
using System.Collections.Generic;
using WeatherApi.Models;

namespace WeatherApi.Mappers
{
    public interface IConverterToShortResponse
    {
        WeatherResponseShortDto ConvertToShortResponse(WeatherResponseDto item);
    }
}