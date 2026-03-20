using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Feedback.Application.Repositories;

public interface ICompletedOrderReplicaRepository : IRepositoryBase<CompletedOrderReplica, Guid>
{
    Task<CompletedOrderReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<CompletedOrderReplica>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
