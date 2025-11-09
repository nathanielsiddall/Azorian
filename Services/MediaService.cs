using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azorian.Data;
using Azorian.Domain.DTOs;
using Azorian.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Azorian.Services
{
    public class MediaService : IMediaService
    {
        private readonly AppDbContext _dbContext;

        public MediaService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MediaAsset> UploadMediaAsset(Guid ownerUserId, UploadMediaDto dto, CancellationToken ct)
        {
            await EnsureUserExists(ownerUserId, ct);
            var now = DateTime.UtcNow;
            var mediaAsset = new MediaAsset
            {
                Id = Guid.NewGuid(),
                OwnerUserId = ownerUserId,
                MediaType = dto.MediaType,
                StoragePath = dto.StoragePath,
                FileName = dto.FileName,
                FileSizeBytes = dto.FileSizeBytes,
                MimeType = dto.MimeType,
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = now
            };

            await _dbContext.MediaAssets.AddAsync(mediaAsset, ct);
            await _dbContext.SaveChangesAsync(ct);
            return mediaAsset;
        }

        public async Task AttachMediaToInstructor(Guid instructorProfileId, Guid mediaAssetId, Guid actingUserId, CancellationToken ct)
        {
            var profile = await _dbContext.InstructorProfiles.FirstOrDefaultAsync(p => p.Id == instructorProfileId, ct);
            if (profile == null)
            {
                throw new InvalidOperationException("Instructor profile not found.");
            }

            if (profile.UserId != actingUserId)
            {
                throw new InvalidOperationException("Acting user is not authorized to manage this instructor profile.");
            }

            var mediaAsset = await _dbContext.MediaAssets.FirstOrDefaultAsync(m => m.Id == mediaAssetId, ct);
            if (mediaAsset == null)
            {
                throw new InvalidOperationException("Media asset not found.");
            }

            if (mediaAsset.OwnerUserId != actingUserId)
            {
                throw new InvalidOperationException("Media asset must be owned by the acting user.");
            }

            var exists = await _dbContext.InstructorMedia.AsNoTracking().AnyAsync(im => im.InstructorProfileId == instructorProfileId && im.MediaAssetId == mediaAssetId, ct);
            if (exists)
            {
                return;
            }

            var instructorMedia = new InstructorMedia
            {
                Id = Guid.NewGuid(),
                InstructorProfileId = instructorProfileId,
                MediaAssetId = mediaAssetId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.InstructorMedia.AddAsync(instructorMedia, ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task AttachMediaToSchoolhouse(Guid schoolhouseId, Guid mediaAssetId, Guid actingUserId, bool isVisibleOnPublicSite, CancellationToken ct)
        {
            await EnsureActingUserCanManageSchoolhouseMedia(schoolhouseId, actingUserId, ct);
            var mediaAsset = await _dbContext.MediaAssets.FirstOrDefaultAsync(m => m.Id == mediaAssetId, ct);
            if (mediaAsset == null)
            {
                throw new InvalidOperationException("Media asset not found.");
            }

            var exists = await _dbContext.SchoolhouseMedia.AsNoTracking().AnyAsync(sm => sm.SchoolhouseId == schoolhouseId && sm.MediaAssetId == mediaAssetId, ct);
            if (exists)
            {
                return;
            }

            var schoolhouseMedia = new SchoolhouseMedia
            {
                Id = Guid.NewGuid(),
                SchoolhouseId = schoolhouseId,
                MediaAssetId = mediaAssetId,
                IsVisibleOnPublicSite = isVisibleOnPublicSite,
                SortOrder = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.SchoolhouseMedia.AddAsync(schoolhouseMedia, ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task AttachMediaToClass(Guid classId, Guid mediaAssetId, Guid actingUserId, bool isVisibleToPublic, bool isVisibleToEnrolledOnly, CancellationToken ct)
        {
            var classEntity = await _dbContext.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == classId, ct);
            if (classEntity == null)
            {
                throw new InvalidOperationException("Class not found.");
            }

            await EnsureActingUserCanManageSchoolhouseMedia(classEntity.SchoolhouseId, actingUserId, ct);
            var mediaAsset = await _dbContext.MediaAssets.FirstOrDefaultAsync(m => m.Id == mediaAssetId, ct);
            if (mediaAsset == null)
            {
                throw new InvalidOperationException("Media asset not found.");
            }

            var exists = await _dbContext.ClassMedia.AsNoTracking().AnyAsync(cm => cm.ClassId == classId && cm.MediaAssetId == mediaAssetId, ct);
            if (exists)
            {
                return;
            }

            var classMedia = new ClassMedia
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                MediaAssetId = mediaAssetId,
                IsVisibleToPublic = isVisibleToPublic,
                IsVisibleToEnrolledOnly = isVisibleToEnrolledOnly,
                SortOrder = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.ClassMedia.AddAsync(classMedia, ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        private async Task EnsureUserExists(Guid userId, CancellationToken ct)
        {
            var exists = await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Id == userId, ct);
            if (!exists)
            {
                throw new InvalidOperationException("User does not exist.");
            }
        }

        private async Task EnsureActingUserCanManageSchoolhouseMedia(Guid schoolhouseId, Guid actingUserId, CancellationToken ct)
        {
            var authorized = await _dbContext.SchoolhouseStaff
                .AsNoTracking()
                .AnyAsync(s => s.SchoolhouseId == schoolhouseId && s.UserId == actingUserId && s.IsActive && (s.StaffRole == StaffRole.Owner || s.StaffRole == StaffRole.Admin), ct);
            if (!authorized)
            {
                throw new InvalidOperationException("Acting user is not authorized to manage media for this schoolhouse.");
            }
        }
    }
}
