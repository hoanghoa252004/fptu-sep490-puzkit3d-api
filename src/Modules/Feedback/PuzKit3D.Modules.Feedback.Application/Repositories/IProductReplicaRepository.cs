using PuzKit3D.Modules.Feedback.Domain.Entities.ProductReplicas;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Feedback.Application.Repositories;

public interface IProductReplicaRepository : IRepositoryBase<ProductReplica, Guid>
{
    Task<ProductReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
