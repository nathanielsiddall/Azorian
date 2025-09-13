namespace Azorian.Models;

/// <summary>
/// Represents an application user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hashed password for authentication.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user is currently suspended.
    /// Suspended users are temporarily prevented from accessing the system.
    /// </summary>
    public bool IsSuspended { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is permanently banned.
    /// Banned users cannot authenticate or access the API.
    /// </summary>
    public bool IsBanned { get; set; }
}
