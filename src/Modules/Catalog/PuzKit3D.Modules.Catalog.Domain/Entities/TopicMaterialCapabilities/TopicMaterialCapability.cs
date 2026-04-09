using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;

public class TopicMaterialCapability : AggregateRoot<TopicMaterialCapabilityId>
{
    public TopicId TopicId { get; private set; } = null!;
    public MaterialId MaterialId { get; private set; } = null!;
    public CapabilityId CapabilityId { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private TopicMaterialCapability(
        TopicMaterialCapabilityId id,
        TopicId topicId,
        MaterialId materialId,
        CapabilityId capabilityId,
        bool isActive) : base(id)
    {
        TopicId = topicId;
        MaterialId = materialId;
        CapabilityId = capabilityId;
        IsActive = isActive;
    }

    private TopicMaterialCapability() : base()
    {
    }

    public static TopicMaterialCapability Create(
        TopicId topicId,
        MaterialId materialId,
        CapabilityId capabilityId,
        bool isActive = false)
    {
        var id = TopicMaterialCapabilityId.Create();
        return new TopicMaterialCapability(id, topicId, materialId, capabilityId, isActive);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
