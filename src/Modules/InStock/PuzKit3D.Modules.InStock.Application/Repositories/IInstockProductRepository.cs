using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockProductRepository : IRepositoryBase<InstockProduct, InstockProductId>
{
    Task<InstockProduct?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<InstockProduct?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<InstockProduct?> GetByIdWithDrivesAsync(InstockProductId id, CancellationToken cancellationToken = default);
}
