using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class CapabilityRepository : ICapabilityRepository
{
    private readonly CatalogDbContext _context;

    public CapabilityRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Capability?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Capabilities
            .FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);
    }

    public Capability FindById(
        CapabilityId id,
        params Expression<Func<Capability, object>>[] includeProperties)
    {
        var query = _context.Capabilities.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(c => c.Id.Equals(id))!;
    }

    public Capability FindSingle(
        Expression<Func<Capability, bool>> predicate,
        params Expression<Func<Capability, object>>[] includeProperties)
    {
        var query = _context.Capabilities.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(predicate)!;
    }

    public IQueryable<Capability> FindAll(
        Expression<Func<Capability, bool>>? predicate,
        params Expression<Func<Capability, object>>[] includeProperties)
    {
        var query = _context.Capabilities.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query;
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
