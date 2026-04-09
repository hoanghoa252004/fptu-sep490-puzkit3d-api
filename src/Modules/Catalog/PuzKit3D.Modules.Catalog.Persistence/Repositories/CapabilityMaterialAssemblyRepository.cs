using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class CapabilityMaterialAssemblyRepository : ICapabilityMaterialAssemblyRepository
{
    private readonly CatalogDbContext _context;

    public CapabilityMaterialAssemblyRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<CapabilityMaterialAssembly?> GetByIdAsync(
        CapabilityMaterialAssemblyId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityMaterialAssemblies
            .FirstOrDefaultAsync(cma => cma.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<CapabilityMaterialAssembly>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityMaterialAssemblies
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CapabilityMaterialAssembly>> FindAsync(
        Expression<Func<CapabilityMaterialAssembly, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityMaterialAssemblies
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(CapabilityMaterialAssembly entity)
    {
        _context.CapabilityMaterialAssemblies.Add(entity);
    }

    public void Update(CapabilityMaterialAssembly entity)
    {
        _context.CapabilityMaterialAssemblies.Update(entity);
    }

    public void Delete(CapabilityMaterialAssembly entity)
    {
        _context.CapabilityMaterialAssemblies.Remove(entity);
    }

    public void DeleteMultiple(List<CapabilityMaterialAssembly> entities)
    {
        _context.CapabilityMaterialAssemblies.RemoveRange(entities);
    }
}
