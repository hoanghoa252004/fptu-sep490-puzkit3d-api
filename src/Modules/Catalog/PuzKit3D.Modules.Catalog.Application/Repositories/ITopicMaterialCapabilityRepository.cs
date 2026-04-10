using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface ITopicMaterialCapabilityRepository : IRepositoryBase<TopicMaterialCapability, TopicMaterialCapabilityId>
{
    Task<bool> ExistsAsync(
        TopicId topicId,
        MaterialId materialId,
        CapabilityId capabilityId,
        CancellationToken cancellationToken = default);
}
