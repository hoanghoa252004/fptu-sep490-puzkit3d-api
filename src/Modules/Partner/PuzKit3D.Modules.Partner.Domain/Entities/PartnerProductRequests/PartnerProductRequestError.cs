using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

public static class PartnerProductRequestError
{
    public static Error InvalidCode() => Error.Validation(
        "PartnerProductRequest.InvalidCode",
        "Partner product request code cannot be empty.");

    public static Error InvalidQuantity() => Error.Validation(
        "PartnerProductRequest.InvalidQuantity",
        "Total requested quantity must be greater than 0.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProductRequest.NotFound",
        $"Partner product request with ID '{id}' was not found.");

    public static Error NotFoundByCode(string code) => Error.NotFound(
        "PartnerProductRequest.NotFoundByCode",
        $"Partner product request with code '{code}' was not found.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "PartnerProductRequest.DuplicateCode",
        $"Partner product request with code '{code}' already exists.");

    public static Error InvalidStatusTransition(PartnerProductRequestStatus currentStatus, PartnerProductRequestStatus newStatus) => Error.Validation(
        "PartnerProductRequest.InvalidStatusTransition",
        $"Cannot transition from '{currentStatus}' to '{newStatus}'. This status transition is not allowed.");

    public static Error InvalidStatus(PartnerProductRequestStatus currentStatus, PartnerProductRequestStatus expectedStatus) => Error.Validation(
        "PartnerProductRequest.InvalidStatus",
        $"Request status is '{currentStatus}' but must be '{expectedStatus}'.");

    public static Error CannotUpdateAfterQuotationCreated() => Error.Validation(
        "PartnerProductRequest.CannotUpdateAfterQuotationCreated",
        $"Cannot update request after quotation has been created.");

    public static Error PermissionDenied() => Error.Unauthorized(
        "PartnerProductRequest.PermissionDenied",
        "You do not have permission to perform this action on the partner product request.");
}
