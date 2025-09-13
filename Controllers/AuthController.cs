using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azorian.Models;
using Azorian.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Azorian.Controllers;

/// <summary>
/// Provides endpoints for user registration and authentication.
/// </summary>
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userService">Service used to manage users.</param>
    /// <param name="configuration">Configuration for retrieving JWT settings.</param>
    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="credentials">The desired username and password.</param>
    /// <returns>The created user information.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public ActionResult<UserDto> Register(UserCredentials credentials)
    {
        var user = _userService.Create(credentials);
        return Ok(ToDto(user));
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="credentials">The username and password.</param>
    /// <returns>A JWT bearer token if authentication succeeds.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<string> Login(UserCredentials credentials)
    {
        var user = _userService.Authenticate(credentials);
        if (user is null)
        {
            return Unauthorized();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(tokenHandler.WriteToken(token));
    }

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        IsSuspended = user.IsSuspended,
        IsBanned = user.IsBanned
    };
}
