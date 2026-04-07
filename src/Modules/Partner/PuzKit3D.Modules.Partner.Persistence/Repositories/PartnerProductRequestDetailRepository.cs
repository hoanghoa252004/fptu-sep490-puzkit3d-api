using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using System.Linq.Expressions;

using PartnerProductRequestDetailId = PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails.PartnerProductRequestDetailId;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerProductRequestDetailRepository : IPartnerProductRequestDetailRepository
{
    private readonly PartnerDbContext _context;

    public PartnerProductRequestDetailRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<PartnerProductRequestDetail?> GetByIdAsync(
        PartnerProductRequestDetailId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductRequestDetails
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductRequestDetail>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductRequestDetails
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductRequestDetail>> FindAsync(
        Expression<Func<PartnerProductRequestDetail, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductRequestDetails
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductRequestDetail>> FindByRequestIdAsync(
        PartnerProductRequestId requestId,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductRequestDetails
            .Where(d => d.PartnerProductRequestId == requestId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(PartnerProductRequestDetail entity)
    {
        _context.PartnerProductRequestDetails.Add(entity);
    }

    public void Update(PartnerProductRequestDetail entity)
    {
        _context.PartnerProductRequestDetails.Update(entity);
    }

    public void Delete(PartnerProductRequestDetail entity)
    {
        _context.PartnerProductRequestDetails.Remove(entity);
    }

    public void DeleteMultiple(List<PartnerProductRequestDetail> entities)
    {
        _context.PartnerProductRequestDetails.RemoveRange(entities);
    }
}
