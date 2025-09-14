using Azorian.Models;
using Azorian.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Azorian.Controllers;

/// <summary>
/// API controller for managing users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves all users with their roles.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> GetUser(int id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Creates a new user without any roles.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        var created = await _userService.CreateUserAsync(user, null); // Replace null with authenticated user id when auth is implemented
        return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing user. Admin only.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> UpdateUser(int id, User user)
    {
        if (id != user.Id)
            return BadRequest();
        var updated = await _userService.UpdateUserAsync(user, null);
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    /// <summary>
    /// Deletes a user. Admin only.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id, null);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
