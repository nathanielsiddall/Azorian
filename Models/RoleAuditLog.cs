using System.ComponentModel.DataAnnotations;

namespace Azorian.Models;

/// <summary>
/// Records actions performed on roles for auditing purposes.
/// </summary>
public class RoleAuditLog
{
    /// <summary>Primary identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Associated role identifier if still present.</summary>
    public Guid? RoleId { get; set; }

    /// <summary>Associated role.</summary>
    public Role? Role { get; set; }

    /// <summary>Name of the role at the time of the action.</summary>
    [MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;

    /// <summary>Action performed (Add, Edit, Delete, Browse, Read).</summary>
    [MaxLength(20)]
    public string Action { get; set; } = string.Empty;

    /// <summary>User who performed the action.</summary>
    public Guid PerformedByUserId { get; set; }

    /// <summary>Time the action occurred.</summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
