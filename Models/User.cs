using System.Collections.Generic;

namespace Azorian.Models;

/// <summary>
/// Represents an application user.
/// </summary>
public class User
{
    /// <summary>
    /// Primary key for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Collection of roles assigned to this user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
