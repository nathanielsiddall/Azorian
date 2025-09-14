using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azorian.Data;
using Azorian.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Controllers;

/// <summary>
/// BREAD operations for authorization roles with audit logging.
/// </summary>
[ApiController]
[Route("1/[controller]")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly AzorianContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesController"/> class.
    /// </summary>
    /// <param name="context">Database context.</param>
    public RolesController(AzorianContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Browse all roles.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Role>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Role>>> Browse()
    {
        var roles = await _context.Roles.ToListAsync();
        await Log("Browse", null);
        return roles;
    }

    /// <summary>
    /// Read a specific role.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Role>> Read(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        await Log("Read", role);
        return role;
    }

    /// <summary>
    /// Add a new role.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Role), StatusCodes.Status201Created)]
    public async Task<ActionResult<Role>> Add(Role role)
    {
        role.Id = Guid.NewGuid();
        _context.Roles.Add(role);
        await Log("Add", role);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Read), new { id = role.Id }, role);
    }

    /// <summary>
    /// Edit an existing role.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(Guid id, Role update)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        role.Name = update.Name;
        await Log("Edit", role);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Delete a role.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        _context.Roles.Remove(role);
        await Log("Delete", role);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task Log(string action, Role? role)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
        {
            return;
        }

        var log = new RoleAuditLog
        {
            Id = Guid.NewGuid(),
            RoleId = role?.Id,
            RoleName = role?.Name ?? string.Empty,
            Action = action,
            PerformedByUserId = Guid.Parse(userIdClaim),
            Timestamp = DateTime.UtcNow
        };

        _context.RoleAuditLogs.Add(log);
        // Do not SaveChanges here; caller will handle it when appropriate
        await Task.CompletedTask;
    }
}
