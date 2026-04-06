using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;

public static class CustomDesignAssetError
{
    public static Error NotFound() =>
        Error.NotFound("CustomDesignAsset.NotFound", "Custom design asset not found");

    public static Error InvalidRequirement(Guid requirementId) =>
        Error.NotFound("CustomDesignAsset.InvalidRequirement", $"Custom design requirement with id {requirementId} not found");

    public static Error InvalidVersion() =>
        Error.Validation("CustomDesignAsset.InvalidVersion", "Version must be greater than 0");
}
