using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class Class
    {
        public Guid Id { get; set; }
        public Guid SchoolhouseId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public decimal PricePerSeat { get; set; }
        public DateTime StartDateTimeUtc { get; set; }
        public DateTime? EndDateTimeUtc { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Schoolhouse Schoolhouse { get; set; }
        public ICollection<ClassMedia> Media { get; set; }
    }

    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Summary)
                .HasMaxLength(2000);

            entity.Property(e => e.PricePerSeat)
                .HasColumnType("decimal(18,2)");

            entity.HasIndex(e => new { e.SchoolhouseId, e.Slug })
                .IsUnique();

            entity.HasOne(e => e.Schoolhouse)
                .WithMany(s => s.Classes)
                .HasForeignKey(e => e.SchoolhouseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
