using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockInventoryRepository : IRepositoryBase<InstockInventory, InstockInventoryId>
{
    Task<InstockInventory?> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
}
