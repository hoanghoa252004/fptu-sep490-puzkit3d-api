using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IDriveReplicaRepository
{
    Task<DriveReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<DriveReplica>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

