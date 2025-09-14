using System;
using Azorian.Data;
using Azorian.Models;
using Microsoft.EntityFrameworkCore;


namespace Azorian.Services;

/// <summary>
/// Service for basic user management.
/// </summary>
public class UserService
{
    private readonly AzorianContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    public UserService(AzorianContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all users with their roles.
    /// </summary>
    public Task<List<User>> GetUsersAsync() => _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    public Task<User?> GetUserAsync(int id) => _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Id == id);

    /// <summary>
    /// Creates a new user without any roles.
    /// </summary>
    public async Task<User> CreateUserAsync(User user, int? performedByUserId = null)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        await LogAsync("CreateUser", user.Id, performedByUserId);
        return user;
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    public async Task<User?> UpdateUserAsync(User user, int? performedByUserId = null)
    {
        var existing = await _context.Users.FindAsync(user.Id);
        if (existing == null)
            return null;

        existing.Name = user.Name;
        await _context.SaveChangesAsync();
        await LogAsync("UpdateUser", existing.Id, performedByUserId);
        return existing;
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    public async Task<bool> DeleteUserAsync(int id, int? performedByUserId = null)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        await LogAsync("DeleteUser", id, performedByUserId);
        return true;
    }

    private async Task LogAsync(string action, int? targetUserId, int? performedByUserId)
    {
        _context.AuditLogs.Add(new AuditLog
        {
            Action = action,
            TargetUserId = targetUserId,
            PerformedByUserId = performedByUserId,
            Timestamp = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }
}
