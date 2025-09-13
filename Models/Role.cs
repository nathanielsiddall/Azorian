using System.Collections.Generic;

namespace Azorian.Models;

/// <summary>
/// Represents a role in the system.
/// </summary>
public class Role
{
    /// <summary>
    /// Primary key for the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the role.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of the role.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Collection of users assigned to this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
