using System;
using System.Threading;
using System.Threading.Tasks;

namespace Azorian.Domain.Services
{
    public interface ISchoolhouseStaffService
    {
        Task AddAdminToSchoolhouse(Guid schoolhouseId, Guid userId, Guid actingUserId, CancellationToken ct);
        Task AddInstructorToSchoolhouse(Guid schoolhouseId, Guid userId, Guid actingUserId, CancellationToken ct);
        Task RemoveStaffFromSchoolhouse(Guid schoolhouseId, Guid userId, Guid actingUserId, CancellationToken ct);
    }
}
