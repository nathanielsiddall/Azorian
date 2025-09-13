namespace Azorian.Models;

/// <summary>
/// Represents user credentials submitted for registration or authentication.
/// </summary>
public class UserCredentials
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the plain-text password. This value is only used transiently
    /// for registration or login and is never stored.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
