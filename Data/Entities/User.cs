using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{

    public sealed class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public string Role { get; set; } = "User";
        public UserRole UserRole { get; set; } = UserRole.Student;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public ICollection<SchoolhouseStaff> SchoolhouseStaff { get; set; }
        public InstructorProfile InstructorProfile { get; set; }

        public ICollection<MediaAsset> MediaAssets { get; set; }

    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);

            entity.Property(e => e.UserRole)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasMany(e => e.SchoolhouseStaff)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.InstructorProfile)
                .WithOne(p => p.User)
                .HasForeignKey<InstructorProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
