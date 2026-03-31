using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerProductRequestRepository : IPartnerProductRequestRepository
{
    private readonly PartnerDbContext _context;

    public PartnerProductRequestRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<PartnerProductRequest?> GetByIdAsync(
        PartnerProductRequestId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductRequests
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductRequest>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductRequests
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProductRequest>> FindAsync(
        Expression<Func<PartnerProductRequest, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProductRequests
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(PartnerProductRequest entity)
    {
        _context.PartnerProductRequests.Add(entity);
    }

    public void Update(PartnerProductRequest entity)
    {
        _context.PartnerProductRequests.Update(entity);
    }

    public void Delete(PartnerProductRequest entity)
    {
        _context.PartnerProductRequests.Remove(entity);
    }

    public void DeleteMultiple(List<PartnerProductRequest> entities)
    {
        _context.PartnerProductRequests.RemoveRange(entities);
    }

    public async Task<PartnerProductRequest?> GetDetailByIdAsync(PartnerProductRequestId id, CancellationToken cancellationToken)
    {
        return await _context.PartnerProductRequests
            .Include(r => r.Details)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
