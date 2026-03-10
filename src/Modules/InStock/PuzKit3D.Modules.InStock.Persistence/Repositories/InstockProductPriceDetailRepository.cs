using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockProductPriceDetailRepository : IInstockProductPriceDetailRepository
{
    private readonly InStockDbContext _context;

    public InstockProductPriceDetailRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<InstockProductPriceDetail?> GetByIdAsync(
        InstockProductPriceDetailId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductPriceDetails
            .FirstOrDefaultAsync(pd => pd.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InstockProductPriceDetail>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductPriceDetails
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockProductPriceDetail>> FindAsync(
        Expression<Func<InstockProductPriceDetail, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductPriceDetails
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<InstockProductPriceDetail?> GetByPriceAndVariantAsync(
        Guid priceId,
        Guid variantId,
        CancellationToken cancellationToken = default)
    {
        var priceIdTyped = InstockPriceId.From(priceId);
        var variantIdTyped = InstockProductVariantId.From(variantId);
        
        return await _context.InstockProductPriceDetails
            .FirstOrDefaultAsync(
                pd => pd.InstockPriceId == priceIdTyped && pd.InstockProductVariantId == variantIdTyped,
                cancellationToken);
    }

    public async Task<IEnumerable<InstockProductPriceDetail>> GetAllByProductVariantIdAsync(
        InstockProductVariantId variantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductPriceDetails
            .Where(pd => pd.InstockProductVariantId == variantId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(InstockProductPriceDetail entity)
    {
        _context.InstockProductPriceDetails.Add(entity);
    }

    public void Update(InstockProductPriceDetail entity)
    {
        _context.InstockProductPriceDetails.Update(entity);
    }

    public void Delete(InstockProductPriceDetail entity)
    {
        _context.InstockProductPriceDetails.Remove(entity);
    }

    public void DeleteMultiple(List<InstockProductPriceDetail> entities)
    {
        _context.InstockProductPriceDetails.RemoveRange(entities);
    }
}
