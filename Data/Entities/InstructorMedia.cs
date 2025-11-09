using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class InstructorMedia
    {
        public Guid Id { get; set; }
        public Guid InstructorProfileId { get; set; }
        public Guid MediaAssetId { get; set; }
        public bool IsVisibleOnSchoolhousePage { get; set; }
        public bool IsVisibleOnPublicInstructorPage { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public InstructorProfile InstructorProfile { get; set; }
        public MediaAsset MediaAsset { get; set; }
    }

    public class InstructorMediaConfiguration : IEntityTypeConfiguration<InstructorMedia>
    {
        public void Configure(EntityTypeBuilder<InstructorMedia> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.InstructorProfile)
                .WithMany(p => p.Media)
                .HasForeignKey(e => e.InstructorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MediaAsset)
                .WithMany(m => m.InstructorMedia)
                .HasForeignKey(e => e.MediaAssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}