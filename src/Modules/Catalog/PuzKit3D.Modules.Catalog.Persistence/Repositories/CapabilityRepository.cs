using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class CapabilityRepository : ICapabilityRepository
{
    private readonly CatalogDbContext _context;

    public CapabilityRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Capability?> GetByIdAsync(
        CapabilityId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Capabilities
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Capability>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Capabilities
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Capability>> FindAsync(
        Expression<Func<Capability, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Capabilities
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Capability?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Capabilities
            .FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);
    }

    public void Add(Capability entity)
    {
        _context.Capabilities.Add(entity);
    }

    public void Update(Capability entity)
    {
        _context.Capabilities.Update(entity);
    }

    public void Delete(Capability entity)
    {
        _context.Capabilities.Remove(entity);
    }

    public void DeleteMultiple(List<Capability> entities)
    {
        _context.Capabilities.RemoveRange(entities);
    }
}
