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

    public async Task<Material?> GetByIdAsync(
        MaterialId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Materials
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Material>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Materials
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Material>> FindAsync(
        Expression<Func<Material, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Materials
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Material?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Materials
            .FirstOrDefaultAsync(m => m.Slug == slug, cancellationToken);
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
