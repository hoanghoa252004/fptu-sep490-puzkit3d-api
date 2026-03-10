using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;

public static class PartnerProductRequestDetailError
{
    public static Error InvalidPrice() => Error.Validation(
        "PartnerProductRequestDetail.InvalidPrice",
        "Reference unit price must be greater than or equal to 0.");

    public static Error InvalidQuantity() => Error.Validation(
        "PartnerProductRequestDetail.InvalidQuantity",
        "Quantity must be greater than 0.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProductRequestDetail.NotFound",
        $"Partner product request detail with ID '{id}' was not found.");
}
