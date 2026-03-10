using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;

public static class PartnerProductQuotationDetailError
{
    public static Error InvalidQuantity() => Error.Validation(
        "PartnerProductQuotationDetail.InvalidQuantity",
        "Quantity must be greater than 0.");

    public static Error InvalidPrice() => Error.Validation(
        "PartnerProductQuotationDetail.InvalidPrice",
        "Unit price must be greater than or equal to 0.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProductQuotationDetail.NotFound",
        $"Partner product quotation detail with ID '{id}' was not found.");
}
