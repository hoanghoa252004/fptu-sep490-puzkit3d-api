using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Feedback.Persistence.Repositories;

internal sealed class CompletedOrderReplicaRepository : ICompletedOrderReplicaRepository
{
    private readonly FeedbackDbContext _context;

    public CompletedOrderReplicaRepository(FeedbackDbContext context)
    {
        _context = context;
    }

    public async Task<CompletedOrderReplica?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CompletedOrderReplicas
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<CompletedOrderReplica>> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CompletedOrderReplicas
            .Where(o => o.ProductId == productId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompletedOrderReplica>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CompletedOrderReplicas
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompletedOrderReplica>> FindAsync(
        Expression<Func<CompletedOrderReplica, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.CompletedOrderReplicas
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(CompletedOrderReplica entity)
    {
        _context.CompletedOrderReplicas.Add(entity);
    }

    public void Update(CompletedOrderReplica entity)
    {
        _context.CompletedOrderReplicas.Update(entity);
    }

    public void Delete(CompletedOrderReplica entity)
    {
        _context.CompletedOrderReplicas.Remove(entity);
    }

    public void DeleteMultiple(List<CompletedOrderReplica> entities)
    {
        _context.CompletedOrderReplicas.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
