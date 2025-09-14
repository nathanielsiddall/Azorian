using Azorian.Data;
using Azorian.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Controllers;

/// <summary>
/// BREAD operations for user accounts.
/// </summary>
[ApiController]
[Route("1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly AzorianContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="context">Database context.</param>
    public UsersController(AzorianContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Browse all users.
    /// </summary>
    /// <returns>Collection of users.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<User>>> Browse()
    {
        return await _context.Users.Include(u => u.Role).ToListAsync();
    }

    /// <summary>
    /// Read a specific user by identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>User matching the identifier.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> Read(Guid id)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    /// <summary>
    /// Add a new user.
    /// </summary>
    /// <param name="request">User details.</param>
    /// <returns>The created user.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public async Task<ActionResult<User>> Add(RegisterRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            RoleId = request.RoleId,
            Status = UserStatus.Active
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        await _context.Entry(user).Reference(u => u.Role).LoadAsync();
        return CreatedAtAction(nameof(Read), new { id = user.Id }, user);
    }

    /// <summary>
    /// Edit an existing user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="update">Updated user details.</param>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(Guid id, User update)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = update.UserName;
        user.Email = update.Email;
        user.FirstName = update.FirstName;
        user.LastName = update.LastName;
        user.RoleId = update.RoleId;
        user.Status = update.Status;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Delete a user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Suspend a user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    [HttpPost("{id}/suspend")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Suspend(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Status = UserStatus.Suspended;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Ban a user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    [HttpPost("{id}/ban")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ban(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Status = UserStatus.Banned;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
