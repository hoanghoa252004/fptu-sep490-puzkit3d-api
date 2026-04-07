using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerProductOrderDetailRepository : IPartnerProductOrderDetailRepository
{
    private readonly PartnerDbContext _context;

    public PartnerProductOrderDetailRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public void Add(PartnerProductOrderDetail entity)
    {
        _context.PartnerProductOrderDetails.Add(entity);
    }

    public void Delete(PartnerProductOrderDetail entity)
    {
        _context.PartnerProductOrderDetails.Remove(entity);
    }

    public void DeleteMultiple(List<PartnerProductOrderDetail> entities)
    {
        _context.PartnerProductOrderDetails.RemoveRange(entities);
    }

    public async Task<IEnumerable<PartnerProductOrderDetail>> FindAsync(Expression<Func<PartnerProductOrderDetail, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrderDetails
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductOrderDetail>> FindByOrderIdAsync(PartnerProductOrderId orderId, CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrderDetails
            .Where(detail => detail.PartnerProductOrderId == orderId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductOrderDetail>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrderDetails
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PartnerProductOrderDetail?> GetByIdAsync(
        PartnerProductOrderDetailId id, 
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductOrderDetails
            .FirstOrDefaultAsync(detail => detail.Id == id, cancellationToken);
    }

    public void Update(PartnerProductOrderDetail entity)
    {
        _context.PartnerProductOrderDetails.Update(entity);
    }
}
