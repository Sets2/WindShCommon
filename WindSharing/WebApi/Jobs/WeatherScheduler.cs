using Quartz;

namespace WebApi.Jobs;

public class WeatherScheduler
{
    public static async void Start(WebApplication builder)
    {
        var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        IJobDetail job = JobBuilder.Create<WeatherRequester>()
            .WithIdentity("WeatherJob", "group1").Build();

        var now = DateTimeOffset.Now;
        var startDateTime = new DateTimeOffset(now.Year, now.Month, now.Day, 23, 59, 59, TimeSpan.Zero);

        ITrigger trigger = TriggerBuilder.Create() // создаем триггер
            .WithIdentity("WeatherTrigger", "group1") // идентифицируем триггер с именем и группой
            .StartAt(startDateTime) // запуск в 23:59:59 текущих суток
            //.StartNow()
            .WithSimpleSchedule(x => x // настраиваем выполнение действия
                .WithIntervalInHours(24) // через 24 часа
                //                .WithIntervalInSeconds(30) 
                .RepeatForever()) // бесконечное повторение
            .Build(); // создаем триггер

        await scheduler.ScheduleJob(job, trigger); // начинаем выполнение работы
    }
}