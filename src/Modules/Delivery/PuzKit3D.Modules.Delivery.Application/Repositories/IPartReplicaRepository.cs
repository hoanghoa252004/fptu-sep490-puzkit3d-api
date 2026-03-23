using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.Repositories;

public interface IPartReplicaRepository
{
    Task<ResultT<PartReplica>> GetByIdAsync(Guid partId, CancellationToken cancellationToken = default);
}