using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class MediaAsset
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public MediaType MediaType { get; set; }
        public string StoragePath { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string MimeType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User OwnerUser { get; set; }
        public ICollection<SchoolhouseMedia> SchoolhouseMedia { get; set; }
        public ICollection<InstructorMedia> InstructorMedia { get; set; }
        public ICollection<ClassMedia> ClassMedia { get; set; }
    }

    public class MediaAssetConfiguration : IEntityTypeConfiguration<MediaAsset>
    {
        public void Configure(EntityTypeBuilder<MediaAsset> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MediaType)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(e => e.StoragePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.MimeType)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Title)
                .HasMaxLength(255);

            entity.HasOne(e => e.OwnerUser)
                .WithMany(u => u.MediaAssets)
                .HasForeignKey(e => e.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}