using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azorian.Data;
using Azorian.Domain.DTOs;
using Azorian.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Controllers;

[ApiController]
[Route("public")]
public class PublicSchoolhouseController : ControllerBase
{
    readonly AppDbContext dbContext;
    readonly ICurrentSchoolhouseContext currentSchoolhouseContext;

    public PublicSchoolhouseController(AppDbContext dbContext, ICurrentSchoolhouseContext currentSchoolhouseContext)
    {
        this.dbContext = dbContext;
        this.currentSchoolhouseContext = currentSchoolhouseContext;
    }

    [HttpGet("index")]
    public async Task<ActionResult<PublicSchoolhouseIndexResponse>> GetIndex(CancellationToken cancellationToken)
    {
        var schoolhouse = await dbContext.Schoolhouses
            .AsNoTracking()
            .Where(s => s.Id == currentSchoolhouseContext.SchoolhouseId)
            .Select(s => new PublicSchoolhouseInfoDto(
                s.Id,
                s.Name,
                s.Slug,
                s.Tagline,
                s.ShortDescription,
                s.LongDescription,
                s.LogoUrl,
                s.HeroImageUrl,
                s.ContactEmail,
                s.ContactPhone,
                s.AddressLine1,
                s.AddressLine2,
                s.City,
                s.State,
                s.PostalCode,
                s.Country))
            .SingleOrDefaultAsync(cancellationToken);

        if (schoolhouse == null)
        {
            return NotFound();
        }

        var media = await dbContext.SchoolhouseMedia
            .AsNoTracking()
            .Where(m => m.SchoolhouseId == schoolhouse.Id && m.IsVisibleOnPublicSite)
            .OrderBy(m => m.SortOrder)
            .Select(m => new PublicSchoolhouseMediaDto(
                m.Id,
                m.SortOrder,
                new PublicMediaAssetDto(
                    m.MediaAsset.Id,
                    m.MediaAsset.MediaType.ToString(),
                    m.MediaAsset.StoragePath,
                    m.MediaAsset.FileName,
                    m.MediaAsset.FileSizeBytes,
                    m.MediaAsset.MimeType,
                    m.MediaAsset.Title,
                    m.MediaAsset.Description)))
            .ToListAsync(cancellationToken);

        var instructors = await dbContext.InstructorProfiles
            .AsNoTracking()
            .Where(profile => dbContext.SchoolhouseStaff.Any(staff => staff.SchoolhouseId == schoolhouse.Id && staff.StaffRole == StaffRole.Instructor && staff.IsActive && staff.UserId == profile.UserId))
            .Select(profile => new PublicInstructorDto(
                profile.Id,
                profile.DisplayName,
                profile.Bio,
                profile.PhotoUrl,
                profile.PublicContactEmail,
                profile.PublicContactPhone,
                profile.Media
                    .Where(media => media.IsVisibleOnSchoolhousePage)
                    .OrderBy(media => media.SortOrder)
                    .Select(media => new PublicInstructorMediaDto(
                        media.Id,
                        media.SortOrder,
                        new PublicMediaAssetDto(
                            media.MediaAsset.Id,
                            media.MediaAsset.MediaType.ToString(),
                            media.MediaAsset.StoragePath,
                            media.MediaAsset.FileName,
                            media.MediaAsset.FileSizeBytes,
                            media.MediaAsset.MimeType,
                            media.MediaAsset.Title,
                            media.MediaAsset.Description)))
                    .ToList()))
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;

        var classes = await dbContext.Classes
            .AsNoTracking()
            .Where(c => c.SchoolhouseId == schoolhouse.Id && c.IsPublished && c.StartDateTimeUtc >= now)
            .OrderBy(c => c.StartDateTimeUtc)
            .Select(c => new PublicClassSummaryDto(
                c.Id,
                c.Title,
                c.Slug,
                c.Summary,
                c.PricePerSeat,
                c.StartDateTimeUtc,
                c.EndDateTimeUtc))
            .ToListAsync(cancellationToken);

        var response = new PublicSchoolhouseIndexResponse(schoolhouse, media, instructors, classes);
        return Ok(response);
    }

    [HttpGet("instructors")]
    public async Task<ActionResult<PublicSchoolhouseInstructorsResponse>> GetInstructors(CancellationToken cancellationToken)
    {
        var schoolhouseExists = await dbContext.Schoolhouses
            .AsNoTracking()
            .AnyAsync(s => s.Id == currentSchoolhouseContext.SchoolhouseId, cancellationToken);

        if (!schoolhouseExists)
        {
            return NotFound();
        }

        var instructors = await dbContext.InstructorProfiles
            .AsNoTracking()
            .Where(profile => dbContext.SchoolhouseStaff.Any(staff => staff.SchoolhouseId == currentSchoolhouseContext.SchoolhouseId && staff.StaffRole == StaffRole.Instructor && staff.IsActive && staff.UserId == profile.UserId))
            .Select(profile => new PublicInstructorDto(
                profile.Id,
                profile.DisplayName,
                profile.Bio,
                profile.PhotoUrl,
                profile.PublicContactEmail,
                profile.PublicContactPhone,
                profile.Media
                    .Where(media => media.IsVisibleOnPublicInstructorPage)
                    .OrderBy(media => media.SortOrder)
                    .Select(media => new PublicInstructorMediaDto(
                        media.Id,
                        media.SortOrder,
                        new PublicMediaAssetDto(
                            media.MediaAsset.Id,
                            media.MediaAsset.MediaType.ToString(),
                            media.MediaAsset.StoragePath,
                            media.MediaAsset.FileName,
                            media.MediaAsset.FileSizeBytes,
                            media.MediaAsset.MimeType,
                            media.MediaAsset.Title,
                            media.MediaAsset.Description)))
                    .ToList()))
            .ToListAsync(cancellationToken);

        return Ok(new PublicSchoolhouseInstructorsResponse(instructors));
    }

    [HttpGet("classes")]
    public async Task<ActionResult<PublicSchoolhouseClassesResponse>> GetClasses([FromQuery] bool futureOnly, CancellationToken cancellationToken)
    {
        var schoolhouseExists = await dbContext.Schoolhouses
            .AsNoTracking()
            .AnyAsync(s => s.Id == currentSchoolhouseContext.SchoolhouseId, cancellationToken);

        if (!schoolhouseExists)
        {
            return NotFound();
        }

        var query = dbContext.Classes
            .AsNoTracking()
            .Where(c => c.SchoolhouseId == currentSchoolhouseContext.SchoolhouseId && c.IsPublished);

        if (futureOnly)
        {
            var now = DateTime.UtcNow;
            query = query.Where(c => c.StartDateTimeUtc >= now);
        }

        var classes = await query
            .OrderBy(c => c.StartDateTimeUtc)
            .Select(c => new PublicClassSummaryDto(
                c.Id,
                c.Title,
                c.Slug,
                c.Summary,
                c.PricePerSeat,
                c.StartDateTimeUtc,
                c.EndDateTimeUtc))
            .ToListAsync(cancellationToken);

        return Ok(new PublicSchoolhouseClassesResponse(classes));
    }

    [HttpGet("about")]
    public async Task<ActionResult<PublicSchoolhouseAboutResponse>> GetAbout(CancellationToken cancellationToken)
    {
        var schoolhouse = await dbContext.Schoolhouses
            .AsNoTracking()
            .Where(s => s.Id == currentSchoolhouseContext.SchoolhouseId)
            .Select(s => new { s.Id, s.LongDescription })
            .SingleOrDefaultAsync(cancellationToken);

        if (schoolhouse == null)
        {
            return NotFound();
        }

        var media = await dbContext.SchoolhouseMedia
            .AsNoTracking()
            .Where(m => m.SchoolhouseId == schoolhouse.Id && m.IsVisibleOnPublicSite)
            .OrderBy(m => m.SortOrder)
            .Select(m => new PublicSchoolhouseMediaDto(
                m.Id,
                m.SortOrder,
                new PublicMediaAssetDto(
                    m.MediaAsset.Id,
                    m.MediaAsset.MediaType.ToString(),
                    m.MediaAsset.StoragePath,
                    m.MediaAsset.FileName,
                    m.MediaAsset.FileSizeBytes,
                    m.MediaAsset.MimeType,
                    m.MediaAsset.Title,
                    m.MediaAsset.Description)))
            .ToListAsync(cancellationToken);

        return Ok(new PublicSchoolhouseAboutResponse(schoolhouse.Id, schoolhouse.LongDescription, media));
    }
}
