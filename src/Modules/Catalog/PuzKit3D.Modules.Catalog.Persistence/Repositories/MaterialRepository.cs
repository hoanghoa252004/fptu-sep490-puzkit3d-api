using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class MaterialRepository : IMaterialRepository
{
    private readonly CatalogDbContext _context;

    public MaterialRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Material?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Materials
            .FirstOrDefaultAsync(m => m.Slug == slug, cancellationToken);
    }

    public Material FindById(
        MaterialId id,
        params Expression<Func<Material, object>>[] includeProperties)
    {
        var query = _context.Materials.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(m => m.Id.Equals(id))!;
    }

    public Material FindSingle(
        Expression<Func<Material, bool>> predicate,
        params Expression<Func<Material, object>>[] includeProperties)
    {
        var query = _context.Materials.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(predicate)!;
    }

    public IQueryable<Material> FindAll(
        Expression<Func<Material, bool>>? predicate,
        params Expression<Func<Material, object>>[] includeProperties)
    {
        var query = _context.Materials.AsQueryable();

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

    public void Add(Material entity)
    {
        _context.Materials.Add(entity);
    }

    public void Update(Material entity)
    {
        _context.Materials.Update(entity);
    }

    public void Delete(Material entity)
    {
        _context.Materials.Remove(entity);
    }

    public void DeleteMultiple(List<Material> entities)
    {
        _context.Materials.RemoveRange(entities);
    }
}
