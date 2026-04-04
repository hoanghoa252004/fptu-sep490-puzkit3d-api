using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

public static class CustomDesignRequirementError
{
    public static Error NotFound() =>
        Error.NotFound("CustomDesignRequirement.NotFound", "Custom design requirement not found");

    public static Error InvalidDifficulty(string difficulty) =>
        Error.Validation("CustomDesignRequirement.InvalidDifficulty", $"Invalid difficulty level: {difficulty}");

    public static Error InvalidMinPartQuantity() =>
        Error.Validation("CustomDesignRequirement.InvalidMinPartQuantity", "Min part quantity must be greater than 0");

    public static Error InvalidMaxPartQuantity() =>
        Error.Validation("CustomDesignRequirement.InvalidMaxPartQuantity", "Max part quantity must be greater than 0");

    public static Error InvalidQuantityRange() =>
        Error.Validation("CustomDesignRequirement.InvalidQuantityRange", "MinPartQuantity cannot be greater than MaxPartQuantity");

    public static Error TopicNotFound(Guid topicId) =>
        Error.NotFound("CustomDesignRequirement.TopicNotFound", $"Topic with id {topicId} not found");

    public static Error MaterialNotFound(Guid materialId) =>
        Error.NotFound("CustomDesignRequirement.MaterialNotFound", $"Material with id {materialId} not found");

    public static Error AssemblyMethodNotFound(Guid assemblyMethodId) =>
        Error.NotFound("CustomDesignRequirement.AssemblyMethodNotFound", $"AssemblyMethod with id {assemblyMethodId} not found");

    public static Error NoCapabilitiesSpecified() =>
        Error.Validation("CustomDesignRequirement.NoCapabilitiesSpecified", "At least one capability must be specified");

    public static Error CapabilityNotFound(Guid capabilityId) =>
        Error.NotFound("CustomDesignRequirement.CapabilityNotFound", $"Capability with id {capabilityId} not found");
}
