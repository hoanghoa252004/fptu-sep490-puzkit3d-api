using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockProductRepository : IInstockProductRepository
{
    private readonly InStockDbContext _context;

    public InstockProductRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<InstockProduct?> GetByIdAsync(
        InstockProductId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProducts
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<InstockProduct?> GetByIdWithPartsAsync(
        InstockProductId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProducts
            .Include(p => p.Parts)
                .ThenInclude(part => part.Pieces)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InstockProduct>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProducts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockProduct>> FindAsync(
        Expression<Func<InstockProduct, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProducts
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<InstockProduct?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProducts
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<InstockProduct?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProducts
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public void Add(InstockProduct entity)
    {
        _context.InstockProducts.Add(entity);
    }

    public void Update(InstockProduct entity)
    {
        _context.InstockProducts.Update(entity);
    }

    public void Delete(InstockProduct entity)
    {
        _context.InstockProducts.Remove(entity);
    }

    public void DeleteMultiple(List<InstockProduct> entities)
    {
        _context.InstockProducts.RemoveRange(entities);
    }
}
