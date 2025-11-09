using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azorian.Data;
using Azorian.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Services
{
    public class SchoolhouseStaffService : ISchoolhouseStaffService
    {
        private readonly AppDbContext _dbContext;

        public SchoolhouseStaffService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAdminToSchoolhouse(Guid schoolhouseId, Guid userId, Guid actingUserId, CancellationToken ct)
        {
            await EnsureActingUserCanManageStaff(schoolhouseId, actingUserId, ct);
            await EnsureUserExists(userId, ct);
            await SetStaffRole(schoolhouseId, userId, StaffRole.Admin, ct);
        }

        public async Task AddInstructorToSchoolhouse(Guid schoolhouseId, Guid userId, Guid actingUserId, CancellationToken ct)
        {
            await EnsureActingUserCanManageStaff(schoolhouseId, actingUserId, ct);
            await EnsureInstructorProfileExists(userId, ct);
            await SetStaffRole(schoolhouseId, userId, StaffRole.Instructor, ct);
        }

        public async Task RemoveStaffFromSchoolhouse(Guid schoolhouseId, Guid userId, Guid actingUserId, CancellationToken ct)
        {
            await EnsureActingUserCanManageStaff(schoolhouseId, actingUserId, ct);
            var staff = await _dbContext.SchoolhouseStaff.FirstOrDefaultAsync(s => s.SchoolhouseId == schoolhouseId && s.UserId == userId, ct);
            if (staff == null)
            {
                throw new InvalidOperationException("Staff member not found for the specified schoolhouse.");
            }

            if (staff.StaffRole == StaffRole.Owner)
            {
                throw new InvalidOperationException("Owners cannot be removed from the schoolhouse.");
            }

            var now = DateTime.UtcNow;
            staff.IsActive = false;
            staff.UpdatedAt = now;

            if (staff.StaffRole == StaffRole.Instructor)
            {
                var mediaToRemove = await _dbContext.SchoolhouseMedia
                    .Include(sm => sm.MediaAsset)
                    .Where(sm => sm.SchoolhouseId == schoolhouseId && sm.MediaAsset.OwnerUserId == userId)
                    .ToListAsync(ct);
                _dbContext.SchoolhouseMedia.RemoveRange(mediaToRemove);
            }

            await _dbContext.SaveChangesAsync(ct);
        }

        private async Task EnsureActingUserCanManageStaff(Guid schoolhouseId, Guid actingUserId, CancellationToken ct)
        {
            var authorized = await _dbContext.SchoolhouseStaff
                .AsNoTracking()
                .AnyAsync(s => s.SchoolhouseId == schoolhouseId && s.UserId == actingUserId && s.IsActive && (s.StaffRole == StaffRole.Owner || s.StaffRole == StaffRole.Admin), ct);
            if (!authorized)
            {
                throw new InvalidOperationException("Acting user is not authorized to manage staff for this schoolhouse.");
            }
        }

        private async Task EnsureUserExists(Guid userId, CancellationToken ct)
        {
            var exists = await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Id == userId, ct);
            if (!exists)
            {
                throw new InvalidOperationException("User does not exist.");
            }
        }

        private async Task EnsureInstructorProfileExists(Guid userId, CancellationToken ct)
        {
            var profileExists = await _dbContext.InstructorProfiles.AsNoTracking().AnyAsync(p => p.UserId == userId, ct);
            if (!profileExists)
            {
                throw new InvalidOperationException("Instructor profile does not exist for the specified user.");
            }
        }

        private async Task SetStaffRole(Guid schoolhouseId, Guid userId, StaffRole role, CancellationToken ct)
        {
            var staff = await _dbContext.SchoolhouseStaff.FirstOrDefaultAsync(s => s.SchoolhouseId == schoolhouseId && s.UserId == userId, ct);
            var now = DateTime.UtcNow;
            if (staff == null)
            {
                staff = new SchoolhouseStaff
                {
                    Id = Guid.NewGuid(),
                    SchoolhouseId = schoolhouseId,
                    UserId = userId,
                    StaffRole = role,
                    IsActive = true,
                    CreatedAt = now
                };
                await _dbContext.SchoolhouseStaff.AddAsync(staff, ct);
            }
            else
            {
                staff.StaffRole = role;
                staff.IsActive = true;
                staff.UpdatedAt = now;
            }

            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
