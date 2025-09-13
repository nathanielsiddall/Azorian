using System.ComponentModel.DataAnnotations;

namespace Azorian.Models;

/// <summary>
/// Login request payload.
/// </summary>
public class LoginRequest
{
    /// <summary>Email address.</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Password.</summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
