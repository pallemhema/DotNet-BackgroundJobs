using BackgroundJobs.Api.BackgroundTasks;

namespace BackgroundJobs.Api.BackgroundTasks;

public class QueuedHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(
        IBackgroundTaskQueue taskQueue,
        ILogger<QueuedHostedService> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Wait for the next available job
                var job = await _taskQueue.DequeueAsync(stoppingToken);

                _logger.LogInformation(
                    "Processing welcome email for {Email}",
                    job.Email);

                // Simulate email processing
                // await Task.Delay(3000, stoppingToken);
                await Task.Delay(10000, stoppingToken);

                _logger.LogInformation(
                    "Welcome email processed for {Email}",
                    job.Email);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing background job.");
            }
        }

        _logger.LogInformation("Queued Hosted Service stopped.");
    }
}