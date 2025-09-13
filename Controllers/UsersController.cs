using Azorian.Models;
using Azorian.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        var created = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
    }
}
