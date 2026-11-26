using BackgroundJobs.Api.BackgroundTasks;
using BackgroundJobs.Api.Data;
using BackgroundJobs.Api.DTOs;
using BackgroundJobs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IBackgroundTaskQueue _taskQueue;

    public UsersController(
        AppDbContext dbContext,
        IBackgroundTaskQueue taskQueue)
    {
        _dbContext = dbContext;
        _taskQueue = taskQueue;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        // Step 1: Check whether the email already exists
        var emailExists = await _dbContext.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            return Conflict("Email is already registered.");
        }

        // Step 2: Create user
        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email
        };

        // Step 3: Save user to PostgreSQL
        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Step 4: Create background job
        var job = new WelcomeEmailJob(
            user.Id,
            user.FullName,
            user.Email
        );

        // Step 5: Add job to background queue
        await _taskQueue.QueueAsync(job, cancellationToken);

        // Step 6: Return response
        return Accepted(new
        {
            Message = "Registration successful. Welcome email queued.",
            UserId = user.Id,
            Email = user.Email
        });
    }
}