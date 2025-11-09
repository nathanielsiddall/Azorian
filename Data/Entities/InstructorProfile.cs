using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class InstructorProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicContactEmail { get; set; }
        public string PublicContactPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User User { get; set; }

        public ICollection<InstructorMedia> Media { get; set; }

    }

    public class InstructorProfileConfiguration : IEntityTypeConfiguration<InstructorProfile>
    {
        public void Configure(EntityTypeBuilder<InstructorProfile> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.PublicContactEmail)
                .HasMaxLength(320);

            entity.Property(e => e.PublicContactPhone)
                .HasMaxLength(50);

            entity.HasIndex(e => e.UserId)
                .IsUnique();

            entity.HasOne(e => e.User)
                .WithOne(u => u.InstructorProfile)
                .HasForeignKey<InstructorProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}