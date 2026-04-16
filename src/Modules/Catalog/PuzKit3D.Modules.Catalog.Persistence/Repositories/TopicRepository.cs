using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class TopicRepository : ITopicRepository
{
    private readonly CatalogDbContext _context;

    public TopicRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Topic?> GetByIdAsync(
        TopicId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Topics
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Topic>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Topics
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Topic>> FindAsync(
        Expression<Func<Topic, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Topics
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Topic?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Topics
            .FirstOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<Topic>> GetByParentIdAsync(
        TopicId parentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Topics
            .Where(t => t.ParentId == parentId)
            .ToListAsync(cancellationToken);
    }

    public void Add(Topic entity)
    {
        _context.Topics.Add(entity);
    }

    public void Update(Topic entity)
    {
        _context.Topics.Update(entity);
    }

    public void Delete(Topic entity)
    {
        _context.Topics.Remove(entity);
    }

    public void DeleteMultiple(List<Topic> entities)
    {
        _context.Topics.RemoveRange(entities);
    }

    public async Task<IEnumerable<Topic>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Topics.AsQueryable();

        if (!isStaffOrManager)
        {
            query = query.Where(p => p.IsActive);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())
            || p.Slug.ToLower().Contains(searchTerm.ToLower())
            || (p.Description != null && p.Description.ToLower().Contains(searchTerm.ToLower())));
        }

        query = ascending 
            ? query.OrderBy(p => p.CreatedAt) 
            : query.OrderByDescending(p => p.CreatedAt);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
