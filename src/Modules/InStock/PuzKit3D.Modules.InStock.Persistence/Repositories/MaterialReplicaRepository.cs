using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class MaterialReplicaRepository : IMaterialReplicaRepository
{
    private readonly InStockDbContext _context;

    public MaterialReplicaRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MaterialReplicas
            .AnyAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MaterialReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MaterialReplicas
            .AsNoTracking()
            .Select(m => new MaterialReplicaSearchDto(m.Id, m.Name, m.Slug))
            .ToListAsync(cancellationToken);
    }
}


