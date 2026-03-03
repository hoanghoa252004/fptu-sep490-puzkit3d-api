using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface ITopicRepository : IRepositoryBase<Topic, TopicId>
{
    Task<Topic?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Topic>> GetByParentIdAsync(TopicId parentId, CancellationToken cancellationToken = default);
}
