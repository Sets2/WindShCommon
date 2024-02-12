using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Settings;

namespace DataAccess.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<DbInitializer> _logger;
        private readonly IOptions<WindSharingSettings> _options;

        public DbInitializer(DataContext dataContext, ILogger<DbInitializer> logger, IOptions<WindSharingSettings> options)
        {
            _dataContext = dataContext;
            _logger = logger;
            _options = options;
        }

        public async Task InitializeDb()
        {
            try
            {
                _logger.LogInformation("Инициализация БД... ");
                await _dataContext.Database.EnsureDeletedAsync();
                if (await _dataContext.Database.EnsureCreatedAsync())
                {
                    await _dataContext.AddRangeAsync(FakeData.Activities);
                    await _dataContext.SaveChangesAsync();

                    await _dataContext.AddRangeAsync(FakeData.UserWinds);
                    await _dataContext.SaveChangesAsync();
                }
                _logger.LogInformation("Инициализация БД завершена");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Ошибка инициализация БД. {e.Message}, {e.InnerException}");
                throw;
            }

            try
            {
                _logger.LogInformation("Проверка наличия каталога для фотографий");
                if (string.IsNullOrWhiteSpace(_options.Value.DirFullPhoto))
                {
                    var dir = _options.Value.DirPhoto;
                    var path = _options.Value.PathPhoto;
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        path = Directory.GetCurrentDirectory();
                        _options.Value.PathPhoto = path;
                    }

                    dir = string.IsNullOrWhiteSpace(dir) ? path : Path.Combine(path, dir);
                    _options.Value.DirFullPhoto = dir;

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Ошибка проверки или создания каталога для фотографий {e.Message}, {e.InnerException}");
                throw;
            }


        }
    }
}