using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Application.Repositories;

public interface ICartQueryRepository
{
    Task<PartnerProductReplica?> GetPartnerProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InStockProductVariantReplica?> GetInStockProductVariantByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InStockInventoryReplica?> GetInStockInventoryByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<InStockProductPriceDetailReplica?> GetActiveInStockPriceDetailByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<InStockProductPriceDetailReplica?> GetInStockPriceDetailByIdAsync(Guid priceDetailId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, InStockProductVariantReplica>> GetInStockProductVariantsByIdsAsync(List<Guid> variantIds, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, PartnerProductReplica>> GetPartnerProductsByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, InStockProductReplica>> GetInStockProductsByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default);
}

