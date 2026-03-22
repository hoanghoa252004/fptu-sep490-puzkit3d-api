using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IMaterialReplicaRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default);
}


