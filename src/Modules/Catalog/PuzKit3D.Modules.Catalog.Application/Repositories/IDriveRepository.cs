using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface IDriveRepository : IRepositoryBase<Drive, DriveId>
{
    Task<IEnumerable<Drive>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default);
}
