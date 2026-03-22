using PuzKit3D.Modules.SupportTicket.Domain.Entities.PartReplicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.Repositories;

public interface IPartReplicaRepository
{
    Task<PartReplica?> GetByIdAsync(Guid partId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(PartReplica replica, CancellationToken cancellationToken = default);
}
