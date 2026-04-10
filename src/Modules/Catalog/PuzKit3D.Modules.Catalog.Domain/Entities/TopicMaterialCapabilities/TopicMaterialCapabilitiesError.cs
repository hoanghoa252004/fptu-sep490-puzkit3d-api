using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;

public static class TopicMaterialCapabilitiesError
{
    public static Error CombinationAlreadyExists => Error.Conflict(
        "TopicMaterialCapability.CombinationAlreadyExists",
        "A TopicMaterialCapability with the same TopicId, MaterialId, and CapabilityId already exists");

    public static Error NotFound(Guid id) => Error.NotFound(
        "TopicMaterialCapability.NotFound",
        $"TopicMaterialCapability with ID '{id}' was not found.");
}
