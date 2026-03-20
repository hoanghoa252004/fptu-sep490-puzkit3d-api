using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.ProductReplicas;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Feedback.Persistence.Repositories;

internal sealed class ProductReplicaRepository : IProductReplicaRepository
{
    private readonly FeedbackDbContext _context;

    public ProductReplicaRepository(FeedbackDbContext context)
    {
        _context = context;
    }

    public async Task<ProductReplica?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ProductReplicas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ProductReplica>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.ProductReplicas
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductReplica>> FindAsync(
        Expression<Func<ProductReplica, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.ProductReplicas
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(ProductReplica entity)
    {
        _context.ProductReplicas.Add(entity);
    }

    public void Update(ProductReplica entity)
    {
        _context.ProductReplicas.Update(entity);
    }

    public void Delete(ProductReplica entity)
    {
        _context.ProductReplicas.Remove(entity);
    }

    public void DeleteMultiple(List<ProductReplica> entities)
    {
        _context.ProductReplicas.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
