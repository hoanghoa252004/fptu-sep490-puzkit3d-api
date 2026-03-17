using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockOrderRepository : IInstockOrderRepository
{
    private readonly InStockDbContext _context;

    public InstockOrderRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<InstockOrder?> GetByIdAsync(
        InstockOrderId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockOrders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<InstockOrder?> GetByIdWithDetailsAsync(
        InstockOrderId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockOrders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InstockOrder>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockOrders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockOrder>> FindAsync(
        Expression<Func<InstockOrder, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockOrders
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<InstockOrder?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockOrders
            .FirstOrDefaultAsync(o => o.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<InstockOrder>> GetOrdersByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockOrders
            .Include(o => o.OrderDetails)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InstockOrder>> GetAllOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.InstockOrders
            .Include(o => o.OrderDetails)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(InstockOrder entity)
    {
        _context.InstockOrders.Add(entity);
    }

    public void Update(InstockOrder entity)
    {
        _context.InstockOrders.Update(entity);
    }

    public void Delete(InstockOrder entity)
    {
        _context.InstockOrders.Remove(entity);
    }

    public void DeleteMultiple(List<InstockOrder> entities)
    {
        _context.InstockOrders.RemoveRange(entities);
    }
}
