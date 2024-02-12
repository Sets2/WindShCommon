using Quartz;

namespace WebApi.Jobs;

public interface IWeatherRequester
{
    Task Execute(IJobExecutionContext context);
}