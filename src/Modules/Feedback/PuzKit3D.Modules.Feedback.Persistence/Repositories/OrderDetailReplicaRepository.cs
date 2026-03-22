using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Feedback.Persistence.Repositories;

internal sealed class OrderDetailReplicaRepository : IOrderDetailReplicaRepository
{
    private readonly FeedbackDbContext _context;

    public OrderDetailReplicaRepository(FeedbackDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDetailReplica?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderDetailReplicas
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<OrderDetailReplica>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderDetailReplicas
            .Where(o => o.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderDetailReplica>> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderDetailReplicas
            .Where(o => o.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderDetailReplica>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderDetailReplicas
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderDetailReplica>> FindAsync(
        Expression<Func<OrderDetailReplica, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderDetailReplicas
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public void Add(OrderDetailReplica entity)
    {
        _context.OrderDetailReplicas.Add(entity);
    }

    public void Update(OrderDetailReplica entity)
    {
        _context.OrderDetailReplicas.Update(entity);
    }

    public void Delete(OrderDetailReplica entity)
    {
        _context.OrderDetailReplicas.Remove(entity);
    }

    public void DeleteMultiple(List<OrderDetailReplica> entities)
    {
        _context.OrderDetailReplicas.RemoveRange(entities);
    }
}
