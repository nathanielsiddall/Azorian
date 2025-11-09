using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class SchoolhouseMedia
    {
        public Guid Id { get; set; }
        public Guid SchoolhouseId { get; set; }
        public Guid MediaAssetId { get; set; }
        public bool IsVisibleOnPublicSite { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Schoolhouse Schoolhouse { get; set; }
        public MediaAsset MediaAsset { get; set; }
    }

    public class SchoolhouseMediaConfiguration : IEntityTypeConfiguration<SchoolhouseMedia>
    {
        public void Configure(EntityTypeBuilder<SchoolhouseMedia> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Schoolhouse)
                .WithMany(s => s.Media)
                .HasForeignKey(e => e.SchoolhouseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MediaAsset)
                .WithMany(m => m.SchoolhouseMedia)
                .HasForeignKey(e => e.MediaAssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}