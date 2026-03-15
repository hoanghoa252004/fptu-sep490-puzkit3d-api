using PuzKit3D.Modules.InStock.Application.Repositories.Dtos;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface ITopicReplicaRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TopicReplicaSearchDto>> GetAllAsync(CancellationToken cancellationToken = default);
}


