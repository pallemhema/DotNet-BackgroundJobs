using BackgroundJobs.Api.Services;

namespace BackgroundJobs.Api.BackgroundTasks;

public class QueuedHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(
        IBackgroundTaskQueue taskQueue,
        IServiceScopeFactory scopeFactory,
        ILogger<QueuedHostedService> logger)
    {
        _taskQueue = taskQueue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var job = await _taskQueue.DequeueAsync(stoppingToken);

                await using var scope = _scopeFactory.CreateAsyncScope();

                var emailService = scope.ServiceProvider
                    .GetRequiredService<IEmailService>();

                _logger.LogInformation(
                    "Processing welcome email for {Email}",
                    job.Email);

                await emailService.SendWelcomeEmailAsync(
                    job, stoppingToken);
            }
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            // Normal application shutdown.
        }

        _logger.LogInformation("Queued Hosted Service stopped.");
    }
}