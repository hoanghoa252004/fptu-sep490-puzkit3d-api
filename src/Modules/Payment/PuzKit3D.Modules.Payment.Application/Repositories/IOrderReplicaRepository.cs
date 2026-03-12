using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Payment.Application.Repositories;

public interface IOrderReplicaRepository : IRepositoryBase<OrderReplica, Guid>
{
}
