using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderReplicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.Repositories;

public interface ICompletedOrderReplicaRepository
{
    Task<ResultT<CompletedOrderReplica>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultT<List<CompletedOrderReplica>>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(CompletedOrderReplica replica, CancellationToken cancellationToken = default);
}
