using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerProductQuotationRepository : IPartnerProductQuotationRepository
{
    private readonly PartnerDbContext _context;

    public PartnerProductQuotationRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public void Add(PartnerProductQuotation entity)
    {
        _context.PartnerProductQuotations.Add(entity);
    }

    public void Delete(PartnerProductQuotation entity)
    {
        _context.PartnerProductQuotations.Remove(entity);
    }

    public void Update(PartnerProductQuotation entity)
    {
        _context.PartnerProductQuotations.Update(entity);
    }

    public void DeleteMultiple(List<PartnerProductQuotation> entities)
    {
        _context.PartnerProductQuotations.RemoveRange(entities);
    }

    public async Task<PartnerProductQuotation?> GetByIdAsync(
        PartnerProductQuotationId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotations
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductQuotation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotations
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductQuotation>> FindAsync(
        Expression<Func<PartnerProductQuotation, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotations
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductQuotation>> GetAllAsync(
        int? status = null,
        string ?searchTerm = null,
        bool ascending = false,
        CancellationToken cancellationToken = default)
    {
        var query = _context.PartnerProductQuotations
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(q => q.Code.ToLower().Contains(searchTerm.ToLower()));
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status.ToString() == status.Value.ToString());
        }

        query = ascending
        ? query.OrderBy(x => x.CreatedAt)
        : query.OrderByDescending(x => x.CreatedAt);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<PartnerProductQuotation?> GetByRequestIdAsync(PartnerProductRequestId requestId, CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductQuotations
            .FirstOrDefaultAsync(q => q.PartnerProductRequestId == requestId, cancellationToken);
    }

    public Task<bool> ExistsByRequestIdAsync(PartnerProductRequestId requestId, CancellationToken cancellationToken = default)
    {
        return _context.PartnerProductQuotations
            .AnyAsync(q => q.PartnerProductRequestId == requestId, cancellationToken);
    }
}
