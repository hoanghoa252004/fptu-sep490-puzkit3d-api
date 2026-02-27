using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class TopicRepository : ITopicRepository
{
    private readonly CatalogDbContext _context;

    public TopicRepository(CatalogDbContext context)
    {
        _context = context;
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

    public Topic FindById(
        TopicId id,
        params Expression<Func<Topic, object>>[] includeProperties)
    {
        var query = _context.Topics.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(t => t.Id.Equals(id))!;
    }

    public Topic FindSingle(
        Expression<Func<Topic, bool>> predicate,
        params Expression<Func<Topic, object>>[] includeProperties)
    {
        var query = _context.Topics.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(predicate)!;
    }

    public IQueryable<Topic> FindAll(
        Expression<Func<Topic, bool>>? predicate,
        params Expression<Func<Topic, object>>[] includeProperties)
    {
        var query = _context.Topics.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query;
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
}
