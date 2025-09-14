using System.ComponentModel.DataAnnotations;

namespace Azorian.Models;

/// <summary>
/// Authorization role that can be assigned to users.
/// </summary>
public class Role
{
    /// <summary>Primary identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Role name.</summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Users assigned to this role.</summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}
