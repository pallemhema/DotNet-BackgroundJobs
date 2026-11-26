using System.ComponentModel.DataAnnotations;

namespace BackgroundJobs.Api.DTOs;

public class RegisterUserRequest
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}