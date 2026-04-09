using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class TopicMaterialCapabilityRepository : ITopicMaterialCapabilityRepository
{
    private readonly CatalogDbContext _context;

    public TopicMaterialCapabilityRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<TopicMaterialCapability?> GetByIdAsync(
        TopicMaterialCapabilityId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.TopicMaterialCapabilities
            .FirstOrDefaultAsync(tmc => tmc.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TopicMaterialCapability>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.TopicMaterialCapabilities
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TopicMaterialCapability>> FindAsync(
        Expression<Func<TopicMaterialCapability, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.TopicMaterialCapabilities
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(TopicMaterialCapability entity)
    {
        _context.TopicMaterialCapabilities.Add(entity);
    }

    public void Update(TopicMaterialCapability entity)
    {
        _context.TopicMaterialCapabilities.Update(entity);
    }

    public void Delete(TopicMaterialCapability entity)
    {
        _context.TopicMaterialCapabilities.Remove(entity);
    }

    public void DeleteMultiple(List<TopicMaterialCapability> entities)
    {
        _context.TopicMaterialCapabilities.RemoveRange(entities);
    }
}
