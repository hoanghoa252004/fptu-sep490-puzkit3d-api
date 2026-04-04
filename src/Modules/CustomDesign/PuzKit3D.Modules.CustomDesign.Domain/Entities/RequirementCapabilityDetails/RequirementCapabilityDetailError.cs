using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;

public static class RequirementCapabilityDetailError
{
    public static Error NotFound() =>
        Error.NotFound("RequirementCapabilityDetail.NotFound", "Requirement capability detail not found");

    public static Error InvalidRequirement(Guid requirementId) =>
        Error.NotFound("RequirementCapabilityDetail.InvalidRequirement", $"Custom design requirement with id {requirementId} not found");

    public static Error InvalidCapability(Guid capabilityId) =>
        Error.NotFound("RequirementCapabilityDetail.InvalidCapability", $"Capability with id {capabilityId} not found");

    public static Error DuplicateCapability(Guid requirementId, Guid capabilityId) =>
        Error.Conflict("RequirementCapabilityDetail.DuplicateCapability", $"Capability {capabilityId} is already associated with requirement {requirementId}");
}
