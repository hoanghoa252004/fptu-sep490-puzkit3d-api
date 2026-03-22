using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class AssemblyMethodReplicaRepository : IAssemblyMethodReplicaRepository
{
    private readonly InStockDbContext _context;

    public AssemblyMethodReplicaRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethodReplicas
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<AssemblyMethodReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethodReplicas
            .AsNoTracking()
            .Select(a => new AssemblyMethodReplicaSearchDto(a.Id, a.Name, a.Slug))
            .ToListAsync(cancellationToken);
    }
}


