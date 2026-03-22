using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Feedback.Application.Repositories;

public interface IOrderDetailReplicaRepository : IRepositoryBase<OrderDetailReplica, Guid>
{
    Task<OrderDetailReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<OrderDetailReplica>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<OrderDetailReplica>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
