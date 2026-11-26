namespace BackgroundJobs.Api.BackgroundTasks;

public record WelcomeEmailJob(
    int UserId,
    string FullName,
    string Email
);