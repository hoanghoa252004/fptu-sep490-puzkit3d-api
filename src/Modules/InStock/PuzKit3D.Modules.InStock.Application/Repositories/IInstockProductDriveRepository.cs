using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockProductDriveRepository : IRepositoryBase<InstockProductDrive, InstockProductDriveId>
{
    Task<IEnumerable<InstockProductDrive>> GetByProductIdAsync(
        InstockProductId productId,
        CancellationToken cancellationToken = default);
}
