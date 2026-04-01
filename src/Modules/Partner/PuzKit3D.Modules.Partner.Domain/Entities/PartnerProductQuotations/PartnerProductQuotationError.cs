using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

public static class PartnerProductQuotationError
{
    public static Error InvalidCode() => Error.Validation(
        "PartnerProductQuotation.InvalidCode",
        "Partner product quotation code cannot be empty.");

    public static Error InvalidAmount() => Error.Validation(
        "PartnerProductQuotation.InvalidAmount",
        "Amount must be greater than or equal to 0.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProductQuotation.NotFound",
        $"Partner product quotation with ID '{id}' was not found.");

    public static Error NotFoundByCode(string code) => Error.NotFound(
        "PartnerProductQuotation.NotFoundByCode",
        $"Partner product quotation with code '{code}' was not found.");

    public static Error DuplicateRequestId(Guid partnerProductRequestId) => Error.Validation(
        "PartnerProductQuotation.DuplicateRequestId",
        $"Partner product quotation with request ID '{partnerProductRequestId}' already exists.");

    public static Error InvalidStatusTransition(PartnerProductQuotationStatus currentStatus, PartnerProductQuotationStatus newStatus) => Error.Validation(
        "PartnerProductQuotation.InvalidStatusTransition",
        $"Cannot transition from '{currentStatus}' to '{newStatus}'. This status transition is not allowed.");
}