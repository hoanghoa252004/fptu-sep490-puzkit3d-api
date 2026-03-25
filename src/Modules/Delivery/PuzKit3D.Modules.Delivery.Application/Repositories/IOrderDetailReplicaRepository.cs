using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.Repositories;

public interface IOrderDetailReplicaRepository
{
    Task<Result> AddAsync(OrderDetailReplica itemReplica, CancellationToken cancellationToken = default);
    Task<Result> AddRangeAsync(List<OrderDetailReplica> itemReplicas, CancellationToken cancellationToken = default);
    Task<OrderDetailReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<OrderDetailReplica>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
