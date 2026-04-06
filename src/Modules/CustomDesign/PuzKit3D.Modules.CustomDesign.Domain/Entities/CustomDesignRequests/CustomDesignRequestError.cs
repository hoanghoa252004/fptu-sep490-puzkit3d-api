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
        Error.Validation("CustomDesignRequest.InvalidDimensions", "Length, width, and height must be greater than or equal to 10mm");

    public static Error InvalidQuantity() =>
        Error.Validation("CustomDesignRequest.InvalidQuantity", "Desired quantity must be greater than 0");

    public static Error InvalidBudget() =>
        Error.Validation("CustomDesignRequest.InvalidBudget", "Target budget must be greater than 0");

    public static Error InvalidDeliveryDate() =>
        Error.Validation("CustomDesignRequest.InvalidDeliveryDate", "Desired delivery date must be in the future");

    public static Error InvalidType() =>
        Error.Validation("CustomDesignRequest.InvalidType", "Type must be 'Sketch' or 'Idea'");

    public static Error NothingToUpdate() =>
        Error.Validation("CustomDesignRequest.NothingToUpdate", "No fields provided for update");

    public static Error SketchesRequiredForSketchType() =>
        Error.Validation("CustomDesignRequest.SketchesRequiredForSketchType", "At least one sketch is required when type is 'Sketch'");

    public static Error CustomerPromptRequiredForIdeaType() =>
        Error.Validation("CustomDesignRequest.CustomerPromptRequiredForIdeaType", "Customer prompt is required when type is 'Idea'");

    public static Error UnauthorizedStatusUpdate() =>
        Error.Forbidden("CustomDesignRequest.UnauthorizedStatusUpdate", "Only staff members can update the status");

    public static Error CustomerCanOnlyUpdateInMissingInformationStatus() =>
        Error.Forbidden("CustomDesignRequest.CustomerCanOnlyUpdateInMissingInformationStatus", "Customers can only update requests when status is MissingInformation");

    public static Error NoteRequiredForStatusChange() =>
        Error.Validation("CustomDesignRequest.NoteRequiredForStatusChange", "Note is required when changing status to Rejected or MissingInformation");

    public static Error InvalidCustomDesignRequestStatus() =>
        Error.Validation("CustomDesignRequest.InvalidCustomDesignRequestStatus", "Invalid status, status mus be: "+ CustomDesignRequestStatusTransition.GetAllValidStatus());
}



