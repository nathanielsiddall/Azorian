using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azorian.Data
{
    public class Schoolhouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Subdomain { get; set; }
        public string Tagline { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string LogoUrl { get; set; }
        public string HeroImageUrl { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsPublished { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<SchoolhouseStaff> Staff { get; set; }
        public ICollection<SchoolhouseMedia> Media { get; set; }

    }

    public class SchoolhouseConfiguration : IEntityTypeConfiguration<Schoolhouse>
    {
        public void Configure(EntityTypeBuilder<Schoolhouse> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Subdomain).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Tagline).HasMaxLength(500);
            entity.Property(e => e.ShortDescription).HasMaxLength(1000);
            entity.Property(e => e.ContactEmail).HasMaxLength(320);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(100);

            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.Subdomain).IsUnique();

            entity
                .HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
