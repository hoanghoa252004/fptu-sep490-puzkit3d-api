using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerProductQuotationDetailRepository : IPartnerProductQuotationDetailRepository
{
    private readonly PartnerDbContext _context;

    public PartnerProductQuotationDetailRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public void Add(PartnerProductQuotationDetail entity)
    {
        _context.PartnerProductQuotationDetails.Add(entity);
    }

    public void Delete(PartnerProductQuotationDetail entity)
    {
        _context.PartnerProductQuotationDetails.Remove(entity);
    }

    public void Update(PartnerProductQuotationDetail entity)
    {
        _context.PartnerProductQuotationDetails.Update(entity);
    }

    public void DeleteMultiple(List<PartnerProductQuotationDetail> entities)
    {
        _context.PartnerProductQuotationDetails.RemoveRange(entities);
    }

    public async Task<PartnerProductQuotationDetail?> GetByIdAsync(
        PartnerProductQuotationDetailId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotationDetails
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductQuotationDetail>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotationDetails
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductQuotationDetail>> FindAsync(
        Expression<Func<PartnerProductQuotationDetail, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotationDetails
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductQuotationDetail>> FindByQuotationIdAsync(
        PartnerProductQuotationId quotationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotationDetails
            .Where(x => x.PartnerProductQuotationId == quotationId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
