namespace Azorian.Models;

/// <summary>
/// Join entity linking users and roles.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Identifier of the user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Identifier of the role.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Navigation to the user.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation to the role.
    /// </summary>
    public Role Role { get; set; } = null!;
}
