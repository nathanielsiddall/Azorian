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
    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
