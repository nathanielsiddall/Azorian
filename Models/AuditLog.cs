using System;

namespace Azorian.Models;

/// <summary>
/// Records actions performed on roles.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Primary key for the audit log entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The action that occurred (CreateRole, UpdateRole, DeleteRole, AssignUser, RemoveUser, etc.).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Identifier of the role associated with the action.
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// Identifier of the user affected by the action (for assignments).
    /// </summary>
    public int? TargetUserId { get; set; }

    /// <summary>
    /// Identifier of the user that performed the action.
    /// </summary>
    public int? PerformedByUserId { get; set; }

    /// <summary>
    /// Time when the action occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
