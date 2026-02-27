using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class AssemblyMethodRepository : IAssemblyMethodRepository
{
    private readonly CatalogDbContext _context;

    public AssemblyMethodRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<AssemblyMethod?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethods
            .FirstOrDefaultAsync(a => a.Slug == slug, cancellationToken);
    }

    public AssemblyMethod FindById(
        AssemblyMethodId id,
        params Expression<Func<AssemblyMethod, object>>[] includeProperties)
    {
        var query = _context.AssemblyMethods.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(a => a.Id.Equals(id))!;
    }

    public AssemblyMethod FindSingle(
        Expression<Func<AssemblyMethod, bool>> predicate,
        params Expression<Func<AssemblyMethod, object>>[] includeProperties)
    {
        var query = _context.AssemblyMethods.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(predicate)!;
    }

    public IQueryable<AssemblyMethod> FindAll(
        Expression<Func<AssemblyMethod, bool>>? predicate,
        params Expression<Func<AssemblyMethod, object>>[] includeProperties)
    {
        var query = _context.AssemblyMethods.AsQueryable();

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
