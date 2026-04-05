using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class AssemblyMethodReplicaRepository : IAssemblyMethodReplicaRepository
{
    private readonly CustomDesignDbContext _context;

    public AssemblyMethodReplicaRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<AssemblyMethodReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethodReplicas.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<AssemblyMethodReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethodReplicas.FirstOrDefaultAsync(a => a.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<AssemblyMethodReplica>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AssemblyMethodReplicas.ToListAsync(cancellationToken);
    }

    public void Add(AssemblyMethodReplica assemblyMethod)
    {
        _context.AssemblyMethodReplicas.Add(assemblyMethod);
    }

    public void Update(AssemblyMethodReplica assemblyMethod)
    {
        _context.AssemblyMethodReplicas.Update(assemblyMethod);
    }

    public void Remove(AssemblyMethodReplica assemblyMethod)
    {
        _context.AssemblyMethodReplicas.Remove(assemblyMethod);
    }
}


