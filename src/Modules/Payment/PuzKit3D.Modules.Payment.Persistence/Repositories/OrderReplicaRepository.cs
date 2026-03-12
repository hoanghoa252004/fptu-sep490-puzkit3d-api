using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Payment.Persistence.Repositories;

internal sealed class OrderReplicaRepository : IOrderReplicaRepository
{
    private readonly PaymentDbContext _context;

    public OrderReplicaRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<OrderReplica?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderReplicas
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<OrderReplica>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderReplicas
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderReplica>> FindAsync(
        Expression<Func<OrderReplica, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderReplicas
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(OrderReplica entity)
    {
        _context.OrderReplicas.Add(entity);
    }

    public void Update(OrderReplica entity)
    {
        _context.OrderReplicas.Update(entity);
    }

    public void Delete(OrderReplica entity)
    {
        _context.OrderReplicas.Remove(entity);
    }

    public void DeleteMultiple(List<OrderReplica> entities)
    {
        _context.OrderReplicas.RemoveRange(entities);
    }
}
