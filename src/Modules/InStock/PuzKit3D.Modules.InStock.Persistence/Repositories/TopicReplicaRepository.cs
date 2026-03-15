using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class TopicReplicaRepository : ITopicReplicaRepository
{
    private readonly InStockDbContext _context;

    public TopicReplicaRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TopicReplicas
            .AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TopicReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TopicReplicas
            .AsNoTracking()
            .Select(t => new TopicReplicaSearchDto(t.Id, t.Name, t.Slug))
            .ToListAsync(cancellationToken);
    }
}


