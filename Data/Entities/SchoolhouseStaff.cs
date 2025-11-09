using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class SchoolhouseStaff
    {
        public Guid Id { get; set; }
        public Guid SchoolhouseId { get; set; }
        public Guid UserId { get; set; }
        public StaffRole StaffRole { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Schoolhouse Schoolhouse { get; set; }
        public User User { get; set; }
    }

    public class SchoolhouseStaffConfiguration : IEntityTypeConfiguration<SchoolhouseStaff>
    {
        public void Configure(EntityTypeBuilder<SchoolhouseStaff> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.StaffRole)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasOne(e => e.Schoolhouse)
                .WithMany(s => s.Staff)
                .HasForeignKey(e => e.SchoolhouseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany(u => u.SchoolhouseStaff)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}