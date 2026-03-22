using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerProductRepository : IPartnerProductRepository
{
    private readonly PartnerDbContext _context;

    public PartnerProductRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<PartnerProduct?> GetByIdAsync(
        PartnerProductId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProducts
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PartnerProduct>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProducts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProduct>> FindAsync(
        Expression<Func<PartnerProduct, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProducts
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PartnerProduct?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProducts
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public void Add(PartnerProduct entity)
    {
        _context.PartnerProducts.Add(entity);
    }

    public void Update(PartnerProduct entity)
    {
        _context.PartnerProducts.Update(entity);
    }

    public void Delete(PartnerProduct entity)
    {
        _context.PartnerProducts.Remove(entity);
    }

    public void DeleteMultiple(List<PartnerProduct> entities)
    {
        _context.PartnerProducts.RemoveRange(entities);
    }
}
