using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class TopicReplicaRepository : ITopicReplicaRepository
{
    private readonly CustomDesignDbContext _context;

    public TopicReplicaRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<TopicReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TopicReplicas.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<TopicReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.TopicReplicas.FirstOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<TopicReplica>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TopicReplicas.ToListAsync(cancellationToken);
    }

    public void Add(TopicReplica topic)
    {
        _context.TopicReplicas.Add(topic);
    }

    public void Update(TopicReplica topic)
    {
        _context.TopicReplicas.Update(topic);
    }

    public void Remove(TopicReplica topic)
    {
        _context.TopicReplicas.Remove(topic);
    }
}