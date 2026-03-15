using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class CapabilityReplicaRepository : ICapabilityReplicaRepository
{
    private readonly InStockDbContext _context;

    public CapabilityReplicaRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityReplicas
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<CapabilityReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityReplicas
            .AsNoTracking()
            .Select(c => new CapabilityReplicaSearchDto(c.Id, c.Name, c.Slug))
            .ToListAsync(cancellationToken);
    }
}


