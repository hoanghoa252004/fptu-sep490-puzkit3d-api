using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

public static class CustomDesignRequestError
{
    public static Error NotFound() =>
        Error.NotFound("CustomDesignRequest.NotFound", "Custom design request not found");

    public static Error InvalidRequirement(Guid requirementId) =>
        Error.NotFound("CustomDesignRequest.InvalidRequirement", $"Custom design requirement with id {requirementId} not found");

    public static Error InvalidPartQuantity() =>
        Error.Validation("CustomDesignRequest.InvalidPartQuantity", "Desired part quantity must be greater than 0");

    public static Error InvalidDimensions() =>
        Error.Validation("CustomDesignRequest.InvalidDimensions", "Length, width, and height must be greater than 0");

    public static Error InvalidQuantity() =>
        Error.Validation("CustomDesignRequest.InvalidQuantity", "Desired quantity must be greater than 0");

    public static Error InvalidBudget() =>
        Error.Validation("CustomDesignRequest.InvalidBudget", "Target budget must be greater than 0");

    public static Error InvalidDeliveryDate() =>
        Error.Validation("CustomDesignRequest.InvalidDeliveryDate", "Desired delivery date cannot be in the past");
}
