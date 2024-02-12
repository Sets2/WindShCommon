using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WeatherApi.Models;

namespace WeatherApi.Services.Hub
{
    public class WeatherHub : Hub<IWeatherHubClient>
    {
        public async Task SubscribeOnGettingWeather(string UserId)
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                Console.WriteLine($"Subscribed {Context.ConnectionId} to {UserId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, UserId);
            }
        }
    }

    public interface IWeatherHubClient
    {
        Task RecieveWeather( WeatherResponseShortDto response);
        Task RecieveSpotsWeather( List<WeatherResponseDto> response); 
    }
}
