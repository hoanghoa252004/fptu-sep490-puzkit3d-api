using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
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

    public async Task<bool> ExistsAsync(
        TopicId topicId, 
        MaterialId materialId, 
        CapabilityId capabilityId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.TopicMaterialCapabilities
            .AnyAsync(tmc => tmc.TopicId == topicId 
                && tmc.MaterialId == materialId 
                && tmc.CapabilityId == capabilityId, cancellationToken);
    }

    public async Task<IEnumerable<TopicMaterialCapability>> GetTopicMaterialCapabilitiesByTopicIdAsync(
        TopicId topicId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.TopicMaterialCapabilities
            .Where(tmc => tmc.TopicId == topicId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TopicMaterialCapability>> GetTopicMaterialCapabilitiesByMaterialIdAsync(
        MaterialId materialId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.TopicMaterialCapabilities
            .Where(tmc => tmc.MaterialId == materialId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TopicMaterialCapability>> GetTopicMaterialCapabilitiesByCapabilityIdAsync(
        CapabilityId capabilityId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.TopicMaterialCapabilities
            .Where(tmc => tmc.CapabilityId == capabilityId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
