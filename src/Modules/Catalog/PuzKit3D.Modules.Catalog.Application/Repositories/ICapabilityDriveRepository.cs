using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface ICapabilityDriveRepository
{
    Task<CapabilityDrive?> GetByIdAsync(CapabilityId capabilityId, DriveId driveId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CapabilityDrive>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CapabilityDrive>> FindAsync(Expression<Func<CapabilityDrive, bool>> predicate, CancellationToken cancellationToken = default);
    void Add(CapabilityDrive entity);
    void Update(CapabilityDrive entity);
    void Delete(CapabilityDrive entity);
    void DeleteMultiple(List<CapabilityDrive> entities);
    Task<CapabilityDrive?> GetByDriveIdAsync(DriveId driveId, CancellationToken cancellationToken = default);
}
