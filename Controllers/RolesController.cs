using Azorian.Models;
using Azorian.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Azorian.Controllers;

/// <summary>
/// API controller for managing roles and user assignments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly RoleService _roleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesController"/> class.
    /// </summary>
    public RolesController(RoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        var roles = await _roleService.GetRolesAsync();
        return Ok(roles);
    }

    /// <summary>
    /// Retrieves a role by identifier.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Role?>> GetRole(int id)
    {
        var role = await _roleService.GetRoleAsync(id);
        if (role == null)
            return NotFound();
        return Ok(role);
    }

    /// <summary>
    /// Creates a new role. Admin only.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Role>> CreateRole(Role role)
    {
        var created = await _roleService.CreateRoleAsync(role, null); // Replace null with authenticated user id when auth is implemented
        return CreatedAtAction(nameof(GetRole), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates a role. Admin only.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Role>> UpdateRole(int id, Role role)
    {
        if (id != role.Id)
            return BadRequest();
        var updated = await _roleService.UpdateRoleAsync(role, null);
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    /// <summary>
    /// Deletes a role. Admin only.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var result = await _roleService.DeleteRoleAsync(id, null);
        if (!result)
            return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Assigns a user to a role. Admin only.
    /// </summary>
    [HttpPost("{roleId}/users/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignUser(int roleId, int userId)
    {
        var result = await _roleService.AssignUserToRoleAsync(roleId, userId, null);
        if (!result)
            return BadRequest();
        return NoContent();
    }

    /// <summary>
    /// Removes a user from a role. Admin only.
    /// </summary>
    [HttpDelete("{roleId}/users/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveUser(int roleId, int userId)
    {
        var result = await _roleService.RemoveUserFromRoleAsync(roleId, userId, null);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
