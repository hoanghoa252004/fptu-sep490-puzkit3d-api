using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Feedback.Application.Repositories;

public interface IOrderReplicaRepository : IRepositoryBase<OrderReplica, Guid>
{
    Task<OrderReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
