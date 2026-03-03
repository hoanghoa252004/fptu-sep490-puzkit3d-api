using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Repositories;

internal sealed class CartQueryRepository : ICartQueryRepository
{
    private readonly CartDbContext _context;

    public CartQueryRepository(CartDbContext context)
    {
        _context = context;
    }

    public async Task<PartnerProductReplica?> GetPartnerProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductReplicas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<InStockProductVariantReplica?> GetInStockProductVariantByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.InStockProductVariantReplicas
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<InStockInventoryReplica?> GetInStockInventoryByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _context.InStockInventoryReplicas
            .FirstOrDefaultAsync(i => i.InStockProductVariantId == variantId, cancellationToken);
    }

    public async Task<InStockProductPriceDetailReplica?> GetActiveInStockPriceDetailByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        
        return await (
            from detail in _context.InStockProductPriceDetailReplicas
            join price in _context.InStockPriceReplicas on detail.InStockPriceId equals price.Id
            where detail.InStockProductVariantId == variantId
                && detail.IsActive
                && price.IsActive
                && price.EffectiveFrom <= now
                && price.EffectiveTo >= now
            orderby price.Priority ascending
            select detail
        ).FirstOrDefaultAsync(cancellationToken);
    }
}
