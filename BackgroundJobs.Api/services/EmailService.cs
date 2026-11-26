using BackgroundJobs.Api.BackgroundTasks;

namespace BackgroundJobs.Api.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendWelcomeEmailAsync(
        WelcomeEmailJob job,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Sending welcome email to {Email}",
            job.Email);

        // Simulate an email provider taking 3 seconds.
        await Task.Delay(3000, cancellationToken);

        // Simulate a provider failure for testing.
        if (job.Email.Contains("fail", StringComparison.OrdinalIgnoreCase))
        {
            throw new Exception("Email provider is unavailable.");
        }

        _logger.LogInformation(
            "Welcome email successfully sent to {Email}",
            job.Email);
    }
}