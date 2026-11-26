using BackgroundJobs.Api.BackgroundTasks;

namespace BackgroundJobs.Api.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(
        WelcomeEmailJob job,
        CancellationToken cancellationToken);
}