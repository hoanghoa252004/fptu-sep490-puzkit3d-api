using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface ICapabilityReplicaRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CapabilityReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default);
}


