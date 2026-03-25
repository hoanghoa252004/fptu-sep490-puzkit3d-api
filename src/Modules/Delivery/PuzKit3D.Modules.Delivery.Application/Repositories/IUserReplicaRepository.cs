using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.Repositories;

public interface IUserReplicaRepository
{
    Task<ResultT<UserReplica>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
