using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class MaterialReplicaRepository : IMaterialReplicaRepository
{
    private readonly CustomDesignDbContext _context;

    public MaterialReplicaRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<MaterialReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MaterialReplicas.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<MaterialReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.MaterialReplicas.FirstOrDefaultAsync(m => m.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<MaterialReplica>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MaterialReplicas.ToListAsync(cancellationToken);
    }

    public void Add(MaterialReplica material)
    {
        _context.MaterialReplicas.Add(material);
    }

    public void Update(MaterialReplica material)
    {
        _context.MaterialReplicas.Update(material);
    }

    public void Remove(MaterialReplica material)
    {
        _context.MaterialReplicas.Remove(material);
    }
}

