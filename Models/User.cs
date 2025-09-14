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

    /// <summary>Identifier of the assigned role, if any.</summary>
    public Guid? RoleId { get; set; }

    /// <summary>Navigation property for the user's role.</summary>
    public Role? Role { get; set; }

    /// <summary>Account status.</summary>
    public UserStatus Status { get; set; } = UserStatus.Active;

    /*TODO: update the isSuspended field so that it's a date so0 that when we create the suspension functuionality
    is made we can just run a check on the date and see if the suspension is over yet.*/
    public bool IsSuspended { get; set; }
    public bool IsBanned { get; set; }
}
