using System.Text;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Auth;
using WebApi.Middlewares;
using Core.Abstractions.Repositories;
using DataAccess.Repositories;
using DataAccess.Data;
using NLog;
using NLog.Web;
using System.Reflection;
using Core.Domain;
using Core.Settings;
using Microsoft.AspNetCore.Authorization;
using WebApi.Settings;
using MassTransit;
using Quartz;
using SharedModel;
using WebApi.Jobs;
using FluentValidation;
using FluentValidation.AspNetCore;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    // builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    builder.Host.UseNLog();

    RegisterServices(builder);

    var app = builder.Build();

    await Configure(app);

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

// Регистрация сервисов
static void RegisterServices(WebApplicationBuilder builder)
{
    IServiceCollection services = builder.Services;

    #region CORS
    services.AddCors(options =>
    {
        options.AddPolicy(name: "MyAllowSpecificOrigins",
                          bldr =>
                          {
                              bldr.WithOrigins(builder.Configuration.GetSection("CORS:Origins").Get<string[]>())
                              .WithHeaders(builder.Configuration.GetSection("CORS:Headers").Get<string[]>())
                              .WithMethods(builder.Configuration.GetSection("CORS:Methods").Get<string[]>());
                          });
    });

    #endregion

    services.Configure<WindSharingSettings>(builder.Configuration.GetSection("WindSharingSettings"));
    services.Configure<WeatherApiSettings>(builder.Configuration.GetSection("WeatherApiSettings"));
    services.AddControllers().AddFluentValidation(options =>
    {
        // Validate child properties and root collection elements
        options.ImplicitlyValidateChildProperties = true;
        options.ImplicitlyValidateRootCollectionElements = true;

        // Automatic registration of validators in assembly
        options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "WindSharing.WebApi", Version = "v1" });
        c.AddSecurityDefinition($"AuthToken",
            new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
                Name = "Authorization",
                Description = "Authorization token"
            });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = $"AuthToken"
                    }
                },
                new string[] { }
            }

        });
        // using System.Reflection;
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    services.AddDbContext<DataContext>(option =>
    {
        option.UseNpgsql(builder.Configuration.GetConnectionString("SqlDb"));
        //        option.UseSnakeCaseNamingConvention();
        option.UseLazyLoadingProxies();

    });
    services.AddIdentity<UserWind, IdentityRole<Guid>>()
        .AddRoles<IdentityRole<Guid>>()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<DataContext>();

    services.AddSingleton<ITokenService, TokenService>();
    services.AddScoped<IUserRepositoryAuth, UserRepositoryAuth>();
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IAuthInit, AuthInit>();
    services.AddScoped<IDbInitializer, DbInitializer>();

    services.AddMvc(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    });

    services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(option =>
        {
            option.SaveToken = true;
            option.RequireHttpsMetadata = false;
            option.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    services.AddAuthorization(opts =>
    {
        // Доступ только пользователей, прошедших проверку подлинности 
        opts.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        // устанавливаем ограничение по роли и Id пользователя 
        opts.AddPolicy(AdminOrOwnerRequirement.Name,
            policy => policy.Requirements.Add(new AdminOrOwnerRequirement()));
        opts.AddPolicy(OwnerRequirement.Name,
            policy => policy.Requirements.Add(new OwnerRequirement()));
    });
    services.AddSingleton<IAuthorizationHandler, CustomHandler>();
    services.AddMassTransit(x =>
    {
        x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
        {
            var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
            c.Host(rabbitMQSettings.Host);
            c.ConfigureEndpoints(context);
        }));

        x.AddRequestClient<WeatherRequest>();
    });
    services.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
    services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });
    services.AddTransient<IWeatherRequester,WeatherRequester>();
}

static async Task Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
    }
    else
    {
        app.UseHttpStatusCodeExceptionMiddleware();
        app.UseHsts();
    }

    //app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("MyAllowSpecificOrigins");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    using var scope = app.Services.CreateScope();
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbInit = services.GetRequiredService<IDbInitializer>();
            await dbInit.InitializeDb();
            var authInit = services.GetRequiredService<IAuthInit>();
            await authInit.UserAuthInit();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during open and initialization");
        }
    }
    WeatherScheduler.Start(app);
}
