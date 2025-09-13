using System.ComponentModel.DataAnnotations;

namespace Azorian.Models;

/// <summary>
/// Registration request payload.
/// </summary>
public class RegisterRequest
{
    /// <summary>Desired username.</summary>
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>Email address.</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Password.</summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>First name.</summary>
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Last name.</summary>
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
}
