using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockProductVariantRepository : IInstockProductVariantRepository
{
    private readonly InStockDbContext _context;

    public InstockProductVariantRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<InstockProductVariant?> GetByIdAsync(
        InstockProductVariantId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductVariants
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InstockProductVariant>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductVariants
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockProductVariant>> FindAsync(
        Expression<Func<InstockProductVariant, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductVariants
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<InstockProductVariant?> GetBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductVariants
            .FirstOrDefaultAsync(v => v.Sku == sku, cancellationToken);
    }

    public async Task<IEnumerable<InstockProductVariant>> GetAllByProductIdAsync(
        InstockProductId productId,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductVariants
            .Where(v => v.InstockProductId == productId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(InstockProductVariant entity)
    {
        _context.InstockProductVariants.Add(entity);
    }

    public void Update(InstockProductVariant entity)
    {
        _context.InstockProductVariants.Update(entity);
    }

    public void Delete(InstockProductVariant entity)
    {
        _context.InstockProductVariants.Remove(entity);
    }

    public void DeleteMultiple(List<InstockProductVariant> entities)
    {
        _context.InstockProductVariants.RemoveRange(entities);
    }
}
