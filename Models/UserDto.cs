namespace Azorian.Models;

/// <summary>
/// Data transfer object returned from user-related endpoints.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user is suspended.
    /// </summary>
    public bool IsSuspended { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is banned.
    /// </summary>
    public bool IsBanned { get; set; }
}
