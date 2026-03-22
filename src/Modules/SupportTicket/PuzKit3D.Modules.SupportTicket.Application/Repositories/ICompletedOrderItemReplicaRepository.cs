using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderItemReplicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.Repositories;

public interface ICompletedOrderItemReplicaRepository
{
    Task<Result> AddAsync(CompletedOrderItemReplica itemReplica, CancellationToken cancellationToken = default);
    Task<Result> AddRangeAsync(List<CompletedOrderItemReplica> itemReplicas, CancellationToken cancellationToken = default);
}
