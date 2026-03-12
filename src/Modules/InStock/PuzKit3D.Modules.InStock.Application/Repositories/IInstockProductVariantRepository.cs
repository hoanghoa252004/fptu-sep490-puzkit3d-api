using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockProductVariantRepository : IRepositoryBase<InstockProductVariant, InstockProductVariantId>
{
    Task<InstockProductVariant?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<InstockProductVariant>> GetAllByProductIdAsync(
        InstockProductId productId, 
        CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, string>> GetProductThumbnailsByVariantIdsAsync(
        List<Guid> variantIds,
        CancellationToken cancellationToken = default);
}

