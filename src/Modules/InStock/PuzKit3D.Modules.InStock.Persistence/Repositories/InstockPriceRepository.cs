using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockPriceRepository : IInstockPriceRepository
{
    private readonly InStockDbContext _context;

    public InstockPriceRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<InstockPrice?> GetByIdAsync(
        InstockPriceId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockPrices
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InstockPrice>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockPrices
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockPrice>> FindAsync(
        Expression<Func<InstockPrice, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockPrices
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockPrice>> GetActivePricesAsync(
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockPrices
            .Where(p => p.IsActive && p.EffectiveFrom <= date && p.EffectiveTo >= date)
            .OrderByDescending(p => p.Priority)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(InstockPrice entity)
    {
        _context.InstockPrices.Add(entity);
    }

    public void Update(InstockPrice entity)
    {
        _context.InstockPrices.Update(entity);
    }

    public void Delete(InstockPrice entity)
    {
        _context.InstockPrices.Remove(entity);
    }

    public void DeleteMultiple(List<InstockPrice> entities)
    {
        _context.InstockPrices.RemoveRange(entities);
    }
}
