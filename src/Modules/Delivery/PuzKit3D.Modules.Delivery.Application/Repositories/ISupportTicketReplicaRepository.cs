using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.Repositories;

public interface ISupportTicketReplicaRepository
{
    Task<ResultT<SupportTicketReplica>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultT<List<SupportTicketDetailReplica>>> GetDetailsByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
}
