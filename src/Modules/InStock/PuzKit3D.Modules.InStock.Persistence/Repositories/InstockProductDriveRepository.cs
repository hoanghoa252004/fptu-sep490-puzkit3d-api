using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockProductDriveRepository : IInstockProductDriveRepository
{
    private readonly InStockDbContext _context;

    public InstockProductDriveRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<InstockProductDrive?> GetByIdAsync(
        InstockProductDriveId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductDrives
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InstockProductDrive>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductDrives
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockProductDrive>> FindAsync(
        Expression<Func<InstockProductDrive, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductDrives
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockProductDrive>> GetByProductIdAsync(
        InstockProductId productId,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockProductDrives
            .Where(d => d.InstockProductId == productId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(InstockProductDrive entity)
    {
        _context.InstockProductDrives.Add(entity);
    }

    public void Update(InstockProductDrive entity)
    {
        _context.InstockProductDrives.Update(entity);
    }

    public void Delete(InstockProductDrive entity)
    {
        _context.InstockProductDrives.Remove(entity);
    }

    public void DeleteMultiple(List<InstockProductDrive> entities)
    {
        _context.InstockProductDrives.RemoveRange(entities);
    }
}
