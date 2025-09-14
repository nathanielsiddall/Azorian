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
    /// Users in the system.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Roles available in the system.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Join table between users and roles.
    /// </summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>
    /// Audit log entries for role operations.
    /// </summary>
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();
    }
}
