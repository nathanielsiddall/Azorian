using Azorian.Data;
using Azorian.Models;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Services;

/// <summary>
/// Service for managing roles and user assignments.
/// </summary>
public class RoleService
{
    private readonly AzorianContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    public RoleService(AzorianContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    public Task<List<Role>> GetRolesAsync() => _context.Roles.AsNoTracking().ToListAsync();

    /// <summary>
    /// Retrieves a role by identifier.
    /// </summary>
    public Task<Role?> GetRoleAsync(int id) => _context.Roles.Include(r => r.UserRoles).ThenInclude(ur => ur.User).FirstOrDefaultAsync(r => r.Id == id);

    /// <summary>
    /// Creates a new role.
    /// </summary>
    public async Task<Role> CreateRoleAsync(Role role, int? performedByUserId = null)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        await LogAsync("CreateRole", role.Id, null, performedByUserId);
        return role;
    }

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    public async Task<Role?> UpdateRoleAsync(Role role, int? performedByUserId = null)
    {
        var existing = await _context.Roles.FindAsync(role.Id);
        if (existing == null)
            return null;

        existing.Name = role.Name;
        existing.Description = role.Description;
        await _context.SaveChangesAsync();
        await LogAsync("UpdateRole", existing.Id, null, performedByUserId);
        return existing;
    }

    /// <summary>
    /// Deletes a role.
    /// </summary>
    public async Task<bool> DeleteRoleAsync(int id, int? performedByUserId = null)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return false;
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        await LogAsync("DeleteRole", id, null, performedByUserId);
        return true;
    }

    /// <summary>
    /// Assigns a user to a role.
    /// </summary>
    public async Task<bool> AssignUserToRoleAsync(int roleId, int userId, int? performedByUserId = null)
    {
        if (await _context.UserRoles.AnyAsync(ur => ur.RoleId == roleId && ur.UserId == userId))
            return false;
        _context.UserRoles.Add(new UserRole { RoleId = roleId, UserId = userId });
        await _context.SaveChangesAsync();
        await LogAsync("AssignUser", roleId, userId, performedByUserId);
        return true;
    }

    /// <summary>
    /// Removes a user from a role.
    /// </summary>
    public async Task<bool> RemoveUserFromRoleAsync(int roleId, int userId, int? performedByUserId = null)
    {
        var entry = await _context.UserRoles.FindAsync(userId, roleId);
        if (entry == null)
            return false;
        _context.UserRoles.Remove(entry);
        await _context.SaveChangesAsync();
        await LogAsync("RemoveUser", roleId, userId, performedByUserId);
        return true;
    }

    private async Task LogAsync(string action, int? roleId, int? targetUserId, int? performedByUserId)
    {
        _context.AuditLogs.Add(new AuditLog
        {
            Action = action,
            RoleId = roleId,
            TargetUserId = targetUserId,
            PerformedByUserId = performedByUserId,
            Timestamp = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }
}
