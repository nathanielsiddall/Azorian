using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class ClassMedia
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid MediaAssetId { get; set; }
        public bool IsVisibleToPublic { get; set; }
        public bool IsVisibleToEnrolledOnly { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public MediaAsset MediaAsset { get; set; }
    }

    public class ClassMediaConfiguration : IEntityTypeConfiguration<ClassMedia>
    {
        public void Configure(EntityTypeBuilder<ClassMedia> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.MediaAsset)
                .WithMany(m => m.ClassMedia)
                .HasForeignKey(e => e.MediaAssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}