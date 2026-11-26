namespace BackgroundJobs.Api.BackgroundTasks;

public interface IBackgroundTaskQueue
{
    ValueTask QueueAsync(
        WelcomeEmailJob job,
        CancellationToken cancellationToken = default);

    ValueTask<WelcomeEmailJob> DequeueAsync(
        CancellationToken cancellationToken);
}