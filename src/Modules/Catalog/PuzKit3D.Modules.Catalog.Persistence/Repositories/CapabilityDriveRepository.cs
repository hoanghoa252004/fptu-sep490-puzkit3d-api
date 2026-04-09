using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class CapabilityDriveRepository : ICapabilityDriveRepository
{
    private readonly CatalogDbContext _context;

    public CapabilityDriveRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<CapabilityDrive?> GetByIdAsync(
        CapabilityId capabilityId,
        DriveId driveId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityDrives
            .FirstOrDefaultAsync(cd => cd.CapabilityId == capabilityId && cd.DriveId == driveId, cancellationToken);
    }

    public async Task<IEnumerable<CapabilityDrive>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityDrives
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CapabilityDrive>> FindAsync(
        Expression<Func<CapabilityDrive, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.CapabilityDrives
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(CapabilityDrive entity)
    {
        _context.CapabilityDrives.Add(entity);
    }

    public void Update(CapabilityDrive entity)
    {
        _context.CapabilityDrives.Update(entity);
    }

    public void Delete(CapabilityDrive entity)
    {
        _context.CapabilityDrives.Remove(entity);
    }

    public void DeleteMultiple(List<CapabilityDrive> entities)
    {
        _context.CapabilityDrives.RemoveRange(entities);
    }
}
