using System;
using System.Collections.Generic;

namespace Azorian.Domain.DTOs;

public record PublicMediaAssetDto(
    Guid Id,
    string MediaType,
    string StoragePath,
    string FileName,
    long FileSizeBytes,
    string MimeType,
    string Title,
    string Description
);

public record PublicSchoolhouseMediaDto(
    Guid Id,
    int SortOrder,
    PublicMediaAssetDto MediaAsset
);

public record PublicInstructorMediaDto(
    Guid Id,
    int SortOrder,
    PublicMediaAssetDto MediaAsset
);

public record PublicInstructorDto(
    Guid InstructorProfileId,
    string DisplayName,
    string Bio,
    string PhotoUrl,
    string PublicContactEmail,
    string PublicContactPhone,
    IReadOnlyList<PublicInstructorMediaDto> Media
);

public record PublicClassSummaryDto(
    Guid Id,
    string Title,
    string Slug,
    string Summary,
    decimal PricePerSeat,
    DateTime StartDateTimeUtc,
    DateTime? EndDateTimeUtc
);

public record PublicSchoolhouseInfoDto(
    Guid Id,
    string Name,
    string Slug,
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
    string Country
);

public record PublicSchoolhouseIndexResponse(
    PublicSchoolhouseInfoDto Schoolhouse,
    IReadOnlyList<PublicSchoolhouseMediaDto> Media,
    IReadOnlyList<PublicInstructorDto> Instructors,
    IReadOnlyList<PublicClassSummaryDto> Classes
);

public record PublicSchoolhouseInstructorsResponse(
    IReadOnlyList<PublicInstructorDto> Instructors
);

public record PublicSchoolhouseClassesResponse(
    IReadOnlyList<PublicClassSummaryDto> Classes
);

public record PublicSchoolhouseAboutResponse(
    Guid SchoolhouseId,
    string LongDescription,
    IReadOnlyList<PublicSchoolhouseMediaDto> Media
);
