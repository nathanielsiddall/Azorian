using Azorian.Models;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Data;

/// <summary>
/// Entity Framework database context for the Azorian application.
/// </summary>
public class AzorianContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AzorianContext"/> class.
    /// </summary>
    /// <param name="options">The options to configure the context.</param>
    public AzorianContext(DbContextOptions<AzorianContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets user accounts.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets or sets authorization roles.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Gets or sets audit logs for role actions.
    /// </summary>
    public DbSet<RoleAuditLog> RoleAuditLogs => Set<RoleAuditLog>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<RoleAuditLog>()
            .HasOne(l => l.Role)
            .WithMany()
            .HasForeignKey(l => l.RoleId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Admin" },
            new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "User" }
        );
    }
}
