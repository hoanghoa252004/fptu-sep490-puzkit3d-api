using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class PartnerRepository : IPartnerRepository
{
    private readonly PartnerDbContext _context;

    public PartnerRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Partners.Partner?> GetByIdAsync(
        PartnerId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Partners
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Partners.Partner>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Partners
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Partners.Partner>> FindAsync(
        Expression<Func<Domain.Entities.Partners.Partner, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Partners
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Partners.Partner?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Partners
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public void Add(Domain.Entities.Partners.Partner entity)
    {
        _context.Partners.Add(entity);
    }

    public void Update(Domain.Entities.Partners.Partner entity)
    {
        _context.Partners.Update(entity);
    }

    public void Delete(Domain.Entities.Partners.Partner entity)
    {
        _context.Partners.Remove(entity);
    }

    public void DeleteMultiple(List<Domain.Entities.Partners.Partner> entities)
    {
        _context.Partners.RemoveRange(entities);
    }

    public Task<Domain.Entities.Partners.Partner?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _context.Partners
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public Task<Domain.Entities.Partners.Partner?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Partners
            .FirstOrDefaultAsync(p => p.ContactEmail == email, cancellationToken);
    }

    public Task<Domain.Entities.Partners.Partner?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return _context.Partners
            .FirstOrDefaultAsync(p => p.ContactPhone == phone, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Partners.Partner>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Partners.AsQueryable();

        if (!isStaffOrManager)
        {
            query = query.Where(p => p.IsActive);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p =>
                p.Slug.Contains(searchTerm.ToLower()) ||
                p.Name.Contains(searchTerm.ToLower()) ||
                p.ContactEmail.Contains(searchTerm.ToLower()) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTerm.ToLower())));
        }

        query = ascending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
