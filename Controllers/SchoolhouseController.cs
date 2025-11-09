using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azorian.Data;
using Azorian.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Controllers;

[ApiController]
[Route("schoolhouses")]
public class SchoolhouseController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public SchoolhouseController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<SchoolhouseSummaryDto[]>> Browse(CancellationToken cancellationToken)
    {
        var schoolhouses = await dbContext.Schoolhouses
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new SchoolhouseSummaryDto(
                s.Id,
                s.Name,
                s.Slug,
                s.IsPublished))
            .ToArrayAsync(cancellationToken);

        return Ok(schoolhouses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SchoolhouseDetailDto>> Read(Guid id, CancellationToken cancellationToken)
    {
        var schoolhouse = await dbContext.Schoolhouses
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SchoolhouseDetailDto(
                s.Id,
                s.Name,
                s.Slug,
                s.Subdomain,
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
                s.Country,
                s.IsPublished,
                s.CreatedByUserId,
                s.CreatedAt,
                s.UpdatedAt))
            .SingleOrDefaultAsync(cancellationToken);

        if (schoolhouse is null)
        {
            return NotFound();
        }

        return Ok(schoolhouse);
    }

    [HttpPost]
    public async Task<ActionResult<SchoolhouseDetailDto>> Add(
        [FromBody] SchoolhouseCreateRequest request,
        CancellationToken cancellationToken)
    {
        var schoolhouse = new Schoolhouse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            Subdomain = request.Subdomain,
            Tagline = request.Tagline,
            ShortDescription = request.ShortDescription,
            LongDescription = request.LongDescription,
            LogoUrl = request.LogoUrl,
            HeroImageUrl = request.HeroImageUrl,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,
            IsPublished = request.IsPublished,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Schoolhouses.Add(schoolhouse);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = await ProjectToDetail(schoolhouse.Id, cancellationToken);
        return CreatedAtAction(nameof(Read), new { id = schoolhouse.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SchoolhouseDetailDto>> Edit(
        Guid id,
        [FromBody] SchoolhouseUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var schoolhouse = await dbContext.Schoolhouses
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (schoolhouse is null)
        {
            return NotFound();
        }

        schoolhouse.Name = request.Name;
        schoolhouse.Slug = request.Slug;
        schoolhouse.Subdomain = request.Subdomain;
        schoolhouse.Tagline = request.Tagline;
        schoolhouse.ShortDescription = request.ShortDescription;
        schoolhouse.LongDescription = request.LongDescription;
        schoolhouse.LogoUrl = request.LogoUrl;
        schoolhouse.HeroImageUrl = request.HeroImageUrl;
        schoolhouse.ContactEmail = request.ContactEmail;
        schoolhouse.ContactPhone = request.ContactPhone;
        schoolhouse.AddressLine1 = request.AddressLine1;
        schoolhouse.AddressLine2 = request.AddressLine2;
        schoolhouse.City = request.City;
        schoolhouse.State = request.State;
        schoolhouse.PostalCode = request.PostalCode;
        schoolhouse.Country = request.Country;
        schoolhouse.IsPublished = request.IsPublished;
        schoolhouse.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = await ProjectToDetail(id, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var schoolhouse = await dbContext.Schoolhouses
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (schoolhouse is null)
        {
            return NotFound();
        }

        dbContext.Schoolhouses.Remove(schoolhouse);
        await dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private async Task<SchoolhouseDetailDto> ProjectToDetail(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Schoolhouses
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SchoolhouseDetailDto(
                s.Id,
                s.Name,
                s.Slug,
                s.Subdomain,
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
                s.Country,
                s.IsPublished,
                s.CreatedByUserId,
                s.CreatedAt,
                s.UpdatedAt))
            .SingleAsync(cancellationToken);
    }
}
