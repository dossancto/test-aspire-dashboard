public class TimerJob(
    IServiceProvider serviceProvider
    ) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var logger = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ILogger<TimerJob>>();

        var count = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Actual count: @count", count++);

            await Task.Delay(1000);
        }
    }
}
