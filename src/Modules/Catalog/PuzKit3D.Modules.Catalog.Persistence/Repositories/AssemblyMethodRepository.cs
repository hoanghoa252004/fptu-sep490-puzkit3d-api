using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class AssemblyMethodRepository : IAssemblyMethodRepository
{
    private readonly CatalogDbContext _context;

    public AssemblyMethodRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<AssemblyMethod?> GetByIdAsync(
        AssemblyMethodId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethods
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<AssemblyMethod>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethods
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AssemblyMethod>> FindAsync(
        Expression<Func<AssemblyMethod, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethods
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<AssemblyMethod?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethods
            .FirstOrDefaultAsync(a => a.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<AssemblyMethod>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AssemblyMethods.AsQueryable();

        if (!isStaffOrManager)
        {
            query = query.Where(a => a.IsActive);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower())
                || a.Slug.ToLower().Contains(searchTerm.ToLower())
                || (a.Description != null && a.Description.ToLower().Contains(searchTerm.ToLower())));
        }

        query = ascending
            ? query.OrderBy(a => a.CreatedAt)
            : query.OrderByDescending(a => a.CreatedAt);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(AssemblyMethod entity)
    {
        _context.AssemblyMethods.Add(entity);
    }

    public void Update(AssemblyMethod entity)
    {
        _context.AssemblyMethods.Update(entity);
    }

    public void Delete(AssemblyMethod entity)
    {
        _context.AssemblyMethods.Remove(entity);
    }

    public void DeleteMultiple(List<AssemblyMethod> entities)
    {
        _context.AssemblyMethods.RemoveRange(entities);
    }
}
