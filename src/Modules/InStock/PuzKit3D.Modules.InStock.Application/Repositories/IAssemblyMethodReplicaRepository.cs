using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IAssemblyMethodReplicaRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssemblyMethodReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default);
}


