using System.ComponentModel.DataAnnotations;

namespace Azorian.Models;

/// <summary>
/// Application user entity.
/// </summary>
public class User
{
    /// <summary>Primary identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Unique username.</summary>
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>User email address.</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Hashed password.</summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>User first name.</summary>
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>User last name.</summary>
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>Date the user was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Date the user was last updated.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>User role for authorization.</summary>
    [MaxLength(50)]
    public string Role { get; set; } = "User";

    /// <summary>Account status.</summary>
    public UserStatus Status { get; set; } = UserStatus.Active;
}
