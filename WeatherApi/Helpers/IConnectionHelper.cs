using StackExchange.Redis;
using System;
namespace  WeatherApi.Helper
{
    public interface IConnectionHelper
    {
        ConnectionMultiplexer getConnection() ;
    }
}