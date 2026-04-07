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

    public async Task<IEnumerable<PartnerProduct>> FindByPartnerIdAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.PartnerProducts
            .Where(p => p.PartnerId.Value == partnerId)
            .ToListAsync(cancellationToken);
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

    public async Task<IEnumerable<PartnerProduct>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        Guid? partnerId,
        CancellationToken cancellationToken = default)
    {
        var query = _context.PartnerProducts.AsQueryable();

        if (!isStaffOrManager)
        {
            query = query.Where(p => p.IsActive);
        }

        if (partnerId.HasValue && partnerId.Value != Guid.Empty)
        {
            query = query.Where(p => p.PartnerId.Value == partnerId.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())
            || p.Slug.ToLower().Contains(searchTerm.ToLower())
            || (p.Description != null && p.Description.ToLower().Contains(searchTerm.ToLower())));
        }

        query = ascending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PartnerProduct>> GetAllByPartnerIdAsync(
        Guid id,
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.PartnerProducts
            .Where(p => p.PartnerId.Value == id)
            .AsQueryable();

        if (!isStaffOrManager)
        {
            query = query.Where(p => p.IsActive);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())
            || p.Slug.ToLower().Contains(searchTerm.ToLower())
            || (p.Description != null && p.Description.ToLower().Contains(searchTerm.ToLower())));
        }

        query = ascending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

}
