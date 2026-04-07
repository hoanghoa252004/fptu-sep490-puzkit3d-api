using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerProductOrderRepository : IPartnerProductOrderRepository
{
    private readonly PartnerDbContext _context;

    public PartnerProductOrderRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public void Add(PartnerProductOrder entity)
    {
        _context.PartnerProductOrders.Add(entity);
    }

    public void Delete(PartnerProductOrder entity)
    {
        _context.PartnerProductOrders.Remove(entity);
    }

    public void Update(PartnerProductOrder entity)
    {
        _context.PartnerProductOrders.Update(entity);
    }

    public void DeleteMultiple(List<PartnerProductOrder> entities)
    {
        _context.PartnerProductOrders.RemoveRange(entities);
    }

    public async Task<PartnerProductOrder?> GetByIdAsync(
        PartnerProductOrderId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<PartnerProductOrder?> GetByIdWithDetailsAsync(
        PartnerProductOrderId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrders
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductOrder>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductOrder>> FindAsync(
        Expression<Func<PartnerProductOrder, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrders
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductOrder>> GetAllAsync(
        string? status = null,
        DateTime? createdAtFrom = null,
        DateTime? createdAtTo = null,
        bool ascending = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.PartnerProductOrders.AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status.ToString() == status);
        }

        if (createdAtFrom.HasValue)
        {
            query = query.Where(o => o.CreatedAt >= createdAtFrom.Value);
        }

        if (createdAtTo.HasValue)
        {
            query = query.Where(o => o.CreatedAt <= createdAtTo.Value);
        }

        query = ascending
            ? query.OrderBy(x => x.CreatedAt)
            : query.OrderByDescending(x => x.CreatedAt);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductOrder>> GetByCustomerIdAsync(
        Guid customerId,
        string? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.PartnerProductOrders
            .Where(o => o.CustomerId == customerId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status.ToString() == status);
        }

        return await query
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PartnerProductOrder?> GetByQuotationIdAsync(
        PartnerProductQuotationId quotationId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrders
            .FirstOrDefaultAsync(o => o.PartnerProductQuotationId == quotationId, cancellationToken);
    }
}
