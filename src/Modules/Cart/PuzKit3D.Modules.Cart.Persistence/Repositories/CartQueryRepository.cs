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

    public async Task<InStockProductPriceDetailReplica?> GetInStockPriceDetailByIdAsync(Guid priceDetailId, CancellationToken cancellationToken = default)
    {
        return await _context.InStockProductPriceDetailReplicas
            .FirstOrDefaultAsync(d => d.Id == priceDetailId, cancellationToken);
    }

    public async Task<Dictionary<Guid, InStockProductVariantReplica>> GetInStockProductVariantsByIdsAsync(List<Guid> variantIds, CancellationToken cancellationToken = default)
    {
        var variants = await _context.InStockProductVariantReplicas
            .Where(v => variantIds.Contains(v.Id))
            .ToListAsync(cancellationToken);

        return variants.ToDictionary(v => v.Id, v => v);
    }

    public async Task<Dictionary<Guid, PartnerProductReplica>> GetPartnerProductsByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default)
    {
        var products = await _context.PartnerProductReplicas
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        return products.ToDictionary(p => p.Id, p => p);
    }

    public async Task<Dictionary<Guid, InStockProductReplica>> GetInStockProductsByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken = default)
    {
        var products = await _context.InStockProductReplicas
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        return products.ToDictionary(p => p.Id, p => p);
    }
}

