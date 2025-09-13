using Azorian.Models;
using Azorian.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Azorian.Controllers;

/// <summary>
/// Provides CRUD operations and administrative actions for users.
/// </summary>
[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">Service used to manage users.</param>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of users.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> Get()
        => Ok(_userService.GetAll().Select(ToDto));

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>User information.</returns>
    [HttpGet("{id}")]
    public ActionResult<UserDto> Get(Guid id)
    {
        var user = _userService.GetById(id);
        return user is null ? NotFound() : Ok(ToDto(user));
    }

    /// <summary>
    /// Creates a new user. Intended for administrative use.
    /// </summary>
    /// <param name="credentials">User credentials.</param>
    /// <returns>The created user.</returns>
    [HttpPost]
    public ActionResult<UserDto> Post(UserCredentials credentials)
    {
        var user = _userService.Create(credentials);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, ToDto(user));
    }

    /// <summary>
    /// Updates an existing user. Replaces all editable fields.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="dto">Updated user information.</param>
    /// <returns>No content if successful.</returns>
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UserDto dto)
    {
        var user = _userService.GetById(id);
        if (user is null)
        {
            return NotFound();
        }

        user.Username = dto.Username;
        user.IsSuspended = dto.IsSuspended;
        user.IsBanned = dto.IsBanned;
        _userService.Update(user);
        return NoContent();
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>No content if deleted.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        return _userService.Delete(id) ? NoContent() : NotFound();
    }

    /// <summary>
    /// Suspends a user, temporarily blocking access.
    /// </summary>
    /// <param name="id">User identifier.</param>
    [HttpPost("{id}/suspend")]
    public IActionResult Suspend(Guid id)
        => _userService.Suspend(id) ? NoContent() : NotFound();

    /// <summary>
    /// Removes a user's suspended status.
    /// </summary>
    /// <param name="id">User identifier.</param>
    [HttpPost("{id}/unsuspend")]
    public IActionResult Unsuspend(Guid id)
        => _userService.Unsuspend(id) ? NoContent() : NotFound();

    /// <summary>
    /// Bans a user permanently.
    /// </summary>
    /// <param name="id">User identifier.</param>
    [HttpPost("{id}/ban")]
    public IActionResult Ban(Guid id)
        => _userService.Ban(id) ? NoContent() : NotFound();

    /// <summary>
    /// Removes a user's banned status.
    /// </summary>
    /// <param name="id">User identifier.</param>
    [HttpPost("{id}/unban")]
    public IActionResult Unban(Guid id)
        => _userService.Unban(id) ? NoContent() : NotFound();

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        IsSuspended = user.IsSuspended,
        IsBanned = user.IsBanned
    };
}
