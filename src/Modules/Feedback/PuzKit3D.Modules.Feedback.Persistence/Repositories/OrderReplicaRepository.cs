using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace PuzKit3D.Modules.Feedback.Persistence.Repositories;

internal class OrderReplicaRepository : IOrderReplicaRepository
{
    private readonly FeedbackDbContext _context;

    public OrderReplicaRepository(FeedbackDbContext context)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderReplica>> FindAsync(
        Expression<Func<OrderReplica, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderReplicas
            .Where(predicate)
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
