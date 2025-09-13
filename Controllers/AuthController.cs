using Azorian.Data;
using Azorian.Models;
using Azorian.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Controllers;

/// <summary>
/// Authentication endpoints for registering and logging in users.
/// </summary>
[ApiController]
[Route("1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AzorianContext _context;
    private readonly TokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="context">Database context.</param>
    /// <param name="tokenService">JWT generation service.</param>
    public AuthController(AzorianContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Registers a new user and returns a JWT.
    /// </summary>
    /// <param name="request">Registration request payload.</param>
    /// <returns>JWT for the created user.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Register(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return BadRequest("Email already in use");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            Role = "User",
            Status = UserStatus.Active
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);
        return Ok(token);
    }

    /// <summary>
    /// Authenticates a user and returns a JWT.
    /// </summary>
    /// <param name="request">Login request payload.</param>
    /// <returns>JWT for the authenticated user.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> Login(LoginRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid credentials");
        }

        if (user.Status == UserStatus.Banned)
        {
            return Forbid();
        }

        var token = _tokenService.GenerateToken(user);
        return Ok(token);
    }
}
