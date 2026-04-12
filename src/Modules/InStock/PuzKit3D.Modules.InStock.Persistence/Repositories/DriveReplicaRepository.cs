using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class DriveReplicaRepository : IDriveReplicaRepository
{
    private readonly InStockDbContext _context;

    public DriveReplicaRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DriveReplica>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _context.DriveReplicas
            .Where(d => ids.Contains(d.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DriveReplicas
            .AnyAsync(m => m.Id == id, cancellationToken);
    }
}
