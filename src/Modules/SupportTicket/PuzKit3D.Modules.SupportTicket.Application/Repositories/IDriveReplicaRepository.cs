using PuzKit3D.Modules.SupportTicket.Domain.Entities;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.Repositories;

public interface IDriveReplicaRepository
{
    Task<DriveReplica?> GetByIdAsync(Guid driveId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(DriveReplica replica, CancellationToken cancellationToken = default);
}
