using MassTransit;
using WeatherApi.Services.Consumers;
using WeatherApi.Services.Weather;
using WeatherApi.Settings;
using WeatherApi.RedisCache;
using WeatherApi.Helper;
using WeatherApi.Services.Hub;
using Microsoft.AspNetCore.Http.Connections;
using WeatherApi.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IGivingWeatherForecast, GivingWeatherForecast>(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["IntegrationSettings:GivingWeatherForecastUrl"]);
    }
);
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.AddControllers();

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("CORS:Origins").Get<string[]>())
              .WithHeaders(builder.Configuration.GetSection("CORS:Headers").Get<string[]>())
              .WithMethods(builder.Configuration.GetSection("CORS:Methods").Get<string[]>())
              .AllowCredentials();
    });
});
#endregion

builder.Services.AddScoped<IConnectionHelper, ConnectionHelper>();
builder.Services.AddScoped<IWeatherCacheService, WeatherCacheService>();
builder.Services.AddScoped<IConverterToShortResponse, ConverterToShortResponse>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<WeatherForecastConsumer>();
    x.AddConsumer<WeatherForecastSpotsConsumer>();

    x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
    {
        var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        c.Host(rabbitMQSettings.Host);
        c.ReceiveEndpoint(rabbitMQSettings.Queue!, e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(2, 3000));
            e.ConfigureConsumer<WeatherForecastConsumer>(context);
            e.ConfigureConsumer<WeatherForecastSpotsConsumer>(context);
        });
    }));
});
builder.Services.AddSignalR(options =>
           {
               options.EnableDetailedErrors = true;
               options.KeepAliveInterval = TimeSpan.FromSeconds(2);
           });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("MyAllowSpecificOrigins");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WeatherHub>("/hubs/weather", options =>
    {
        options.ApplicationMaxBufferSize = 1 * 1024 * 1024;

        options.Transports = HttpTransportType.LongPolling |
                            HttpTransportType.WebSockets |
                            HttpTransportType.ServerSentEvents;

        options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
    });
});

app.Run();
