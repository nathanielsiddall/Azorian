using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azorian.Models;
using Microsoft.IdentityModel.Tokens;

namespace Azorian.Services;

/// <summary>
/// Generates JSON Web Tokens for authenticated users.
/// </summary>
public class TokenService
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    /// <param name="config">Application configuration.</param>
    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Generates a signed JWT for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">Authenticated user.</param>
    /// <returns>Signed JWT string.</returns>
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
