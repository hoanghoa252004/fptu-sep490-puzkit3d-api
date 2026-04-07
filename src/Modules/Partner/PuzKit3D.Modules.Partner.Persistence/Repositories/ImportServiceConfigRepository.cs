using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Partner.Persistence.Repositories;

internal sealed class ImportServiceConfigRepository : IImportServiceConfigRepository
{
    private readonly PartnerDbContext _context;

    public ImportServiceConfigRepository(PartnerDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.ImportServiceConfigs.ImportServiceConfig?> GetByIdAsync(
        ImportServiceConfigId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ImportServiceConfigs
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.ImportServiceConfigs.ImportServiceConfig>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.ImportServiceConfigs
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.ImportServiceConfigs.ImportServiceConfig>> FindAsync(
        Expression<Func<Domain.Entities.ImportServiceConfigs.ImportServiceConfig, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.ImportServiceConfigs
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(Domain.Entities.ImportServiceConfigs.ImportServiceConfig entity)
    {
        _context.ImportServiceConfigs.Add(entity);
    }

    public void Update(Domain.Entities.ImportServiceConfigs.ImportServiceConfig entity)
    {
        _context.ImportServiceConfigs.Update(entity);
    }

    public void Delete(Domain.Entities.ImportServiceConfigs.ImportServiceConfig entity)
    {
        _context.ImportServiceConfigs.Remove(entity);
    }

    public void DeleteMultiple(List<Domain.Entities.ImportServiceConfigs.ImportServiceConfig> entities)
    {
        _context.ImportServiceConfigs.RemoveRange(entities);
    }

    public async Task<ImportServiceConfig?> GetByCountryCodeAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        return await _context.ImportServiceConfigs
            .FirstOrDefaultAsync(c => c.CountryCode == countryCode, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.ImportServiceConfigs.ImportServiceConfig>> GetAllAsync(
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ImportServiceConfigs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            query = query.Where(c =>
                c.CountryName.ToLower().Contains(lowerSearchTerm) ||
                c.CountryCode.ToLower().Contains(lowerSearchTerm));
        }

        query = ascending ? query.OrderBy(c => c.CreatedAt) : query.OrderByDescending(c => c.CreatedAt);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
