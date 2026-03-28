using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface ISupportTicketReplicaRepository
{
    Task<ResultT<List<SupportTicketReplica>>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
