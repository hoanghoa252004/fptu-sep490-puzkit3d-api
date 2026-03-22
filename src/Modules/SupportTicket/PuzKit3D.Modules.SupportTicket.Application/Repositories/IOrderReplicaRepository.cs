using PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.Repositories;

public interface IOrderReplicaRepository
{
    Task<ResultT<OrderReplica>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultT<List<OrderReplica>>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(OrderReplica replica, CancellationToken cancellationToken = default);
}
