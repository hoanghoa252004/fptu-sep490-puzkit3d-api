using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class CapabilityReplicaRepository : ICapabilityReplicaRepository
{
    private readonly CustomDesignDbContext _context;

    public CapabilityReplicaRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<CapabilityReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityReplicas.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<CapabilityReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityReplicas.FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<CapabilityReplica>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityReplicas.ToListAsync(cancellationToken);
    }

    public void Add(CapabilityReplica capability)
    {
        _context.CapabilityReplicas.Add(capability);
    }

    public void Update(CapabilityReplica capability)
    {
        _context.CapabilityReplicas.Update(capability);
    }

    public void Remove(CapabilityReplica capability)
    {
        _context.CapabilityReplicas.Remove(capability);
    }
}

