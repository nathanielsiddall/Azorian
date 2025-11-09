using System;

namespace Azorian.Domain.DTOs
{
    public record SchoolhouseSummaryDto(
        Guid Id,
        string Name,
        string Slug,
        bool IsPublished);

    public record SchoolhouseDetailDto(
        Guid Id,
        string Name,
        string Slug,
        string Subdomain,
        string Tagline,
        string ShortDescription,
        string LongDescription,
        string LogoUrl,
        string HeroImageUrl,
        string ContactEmail,
        string ContactPhone,
        string AddressLine1,
        string AddressLine2,
        string City,
        string State,
        string PostalCode,
        string Country,
        bool IsPublished,
        Guid CreatedByUserId,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public record SchoolhouseCreateRequest(
        string Name,
        string Slug,
        string Subdomain,
        string Tagline,
        string ShortDescription,
        string LongDescription,
        string LogoUrl,
        string HeroImageUrl,
        string ContactEmail,
        string ContactPhone,
        string AddressLine1,
        string AddressLine2,
        string City,
        string State,
        string PostalCode,
        string Country,
        bool IsPublished,
        Guid CreatedByUserId);

    public record SchoolhouseUpdateRequest(
        string Name,
        string Slug,
        string Subdomain,
        string Tagline,
        string ShortDescription,
        string LongDescription,
        string LogoUrl,
        string HeroImageUrl,
        string ContactEmail,
        string ContactPhone,
        string AddressLine1,
        string AddressLine2,
        string City,
        string State,
        string PostalCode,
        string Country,
        bool IsPublished);
}
