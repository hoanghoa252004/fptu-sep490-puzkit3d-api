using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Domain.Entities.Orders;
using PuzKit3D.Modules.InStock.Domain.Repositories;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly InStockDbContext _context;

    public OrderRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public Order FindById(OrderId id, params Expression<Func<Order, object>>[] includeProperties)
    {
        var query = _context.Orders.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(o => o.Id.Equals(id))!;
    }

    public Order FindSingle(Expression<Func<Order, bool>> predicate, params Expression<Func<Order, object>>[] includeProperties)
    {
        var query = _context.Orders.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(predicate)!;
    }

    public IQueryable<Order> FindAll(Expression<Func<Order, bool>>? predicate, params Expression<Func<Order, object>>[] includeProperties)
    {
        var query = _context.Orders.AsQueryable();

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

    public void Add(Order entity)
    {
        _context.Orders.Add(entity);
    }

    public void Update(Order entity)
    {
        _context.Orders.Update(entity);
    }

    public void Delete(Order entity)
    {
        _context.Orders.Remove(entity);
    }

    public void DeleteMultiple(List<Order> entities)
    {
        _context.Orders.RemoveRange(entities);
    }
}
