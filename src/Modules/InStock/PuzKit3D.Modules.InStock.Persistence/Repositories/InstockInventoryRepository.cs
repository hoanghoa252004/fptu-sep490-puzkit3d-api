using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockInventoryRepository : IInstockInventoryRepository
{
    private readonly InStockDbContext _context;

    public InstockInventoryRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<InstockInventory?> GetByIdAsync(
        InstockInventoryId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockInventories
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InstockInventory>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockInventories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockInventory>> FindAsync(
        Expression<Func<InstockInventory, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockInventories
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<InstockInventory?> GetByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken = default)
    {
        var variantIdObj = InstockProductVariantId.From(variantId);
        return await _context.InstockInventories
            .FirstOrDefaultAsync(i => i.InstockProductVariantId == variantIdObj, cancellationToken);
    }

    public void Add(InstockInventory entity)
    {
        _context.InstockInventories.Add(entity);
    }

    public void Update(InstockInventory entity)
    {
        _context.InstockInventories.Update(entity);
    }

    public void Delete(InstockInventory entity)
    {
        _context.InstockInventories.Remove(entity);
    }

    public void DeleteMultiple(List<InstockInventory> entities)
    {
        _context.InstockInventories.RemoveRange(entities);
    }
}
