using System;
using System.Threading;
using System.Threading.Tasks;
using Azorian.Domain.DTOs;
using Azorian.Data;

namespace Azorian.Domain.Services
{
    public interface IMediaService
    {
        Task<MediaAsset> UploadMediaAsset(Guid ownerUserId, UploadMediaDto dto, CancellationToken ct);
        Task AttachMediaToInstructor(Guid instructorProfileId, Guid mediaAssetId, Guid actingUserId, CancellationToken ct);
        Task AttachMediaToSchoolhouse(Guid schoolhouseId, Guid mediaAssetId, Guid actingUserId, bool isVisibleOnPublicSite, CancellationToken ct);
        Task AttachMediaToClass(Guid classId, Guid mediaAssetId, Guid actingUserId, bool isVisibleToPublic, bool isVisibleToEnrolledOnly, CancellationToken ct);
    }
}
