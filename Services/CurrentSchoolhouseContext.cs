using System;
using System.Linq;
using Azorian.Data;
using Azorian.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Services;

public class CurrentSchoolhouseContext : ICurrentSchoolhouseContext
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Schoolhouse? _schoolhouse;

    public CurrentSchoolhouseContext(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? SchoolhouseId => Schoolhouse?.Id;

    public Schoolhouse? Schoolhouse => _schoolhouse ??= ResolveSchoolhouse();

    private Schoolhouse? ResolveSchoolhouse()
    {
        var slug = GetSchoolhouseSlugFromRequest();

        var query = _dbContext.Schoolhouses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(slug))
        {
            var schoolhouseBySlug = query.FirstOrDefault(s => s.Slug == slug);
            if (schoolhouseBySlug != null)
            {
                return schoolhouseBySlug;
            }
        }

        return query.OrderBy(s => s.Name).FirstOrDefault();
    }

    private string? GetSchoolhouseSlugFromRequest()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        if (httpContext.Items.TryGetValue("CurrentSchoolhouseSlug", out var itemValue) && itemValue is string itemSlug && !string.IsNullOrWhiteSpace(itemSlug))
        {
            return itemSlug;
        }

        if (httpContext.Request.RouteValues.TryGetValue("schoolhouseSlug", out var routeValue) && routeValue is string routeSlug && !string.IsNullOrWhiteSpace(routeSlug))
        {
            return routeSlug;
        }

        if (httpContext.Request.Query.TryGetValue("schoolhouseSlug", out var querySlug) && !string.IsNullOrWhiteSpace(querySlug))
        {
            return querySlug.ToString();
        }

        if (httpContext.Request.Headers.TryGetValue("X-Schoolhouse-Slug", out var headerSlug) && !string.IsNullOrWhiteSpace(headerSlug))
        {
            return headerSlug.ToString();
        }

        return null;
    }
}
