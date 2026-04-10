using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class DriveRepository : IDriveRepository
{
    private readonly CatalogDbContext _context;

    public DriveRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Drive?> GetByIdAsync(
        DriveId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Drives
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Drive>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Drives
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Drive>> FindAsync(
        Expression<Func<Drive, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Drives
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Drive>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Drives.AsQueryable();

        if (!isStaffOrManager)
        {
            query = query.Where(d => d.IsActive);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(d => d.Name.ToLower().Contains(searchTerm.ToLower())
                || (d.Description != null && d.Description.ToLower().Contains(searchTerm.ToLower())));
        }

        query = ascending
            ? query.OrderBy(d => d.CreatedAt)
            : query.OrderByDescending(d => d.CreatedAt);

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(Drive entity)
    {
        _context.Drives.Add(entity);
    }

    public void Update(Drive entity)
    {
        _context.Drives.Update(entity);
    }

    public void Delete(Drive entity)
    {
        _context.Drives.Remove(entity);
    }

    public void DeleteMultiple(List<Drive> entities)
    {
        _context.Drives.RemoveRange(entities);
    }
}
