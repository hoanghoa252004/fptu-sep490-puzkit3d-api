using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Application.Repositories;

public interface ICartQueryRepository
{
    Task<PartnerProductReplica?> GetPartnerProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InStockProductVariantReplica?> GetInStockProductVariantByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InStockInventoryReplica?> GetInStockInventoryByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<InStockProductPriceDetailReplica?> GetActiveInStockPriceDetailByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
}
