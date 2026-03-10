using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public static class PartnerProductOrderDetailError
{
    public static Error InvalidQuantity() => Error.Validation(
        "PartnerProductOrderDetail.InvalidQuantity",
        "Quantity must be greater than 0.");

    public static Error InvalidPrice() => Error.Validation(
        "PartnerProductOrderDetail.InvalidPrice",
        "Unit price must be greater than or equal to 0.");

    public static Error InvalidSku() => Error.Validation(
        "PartnerProductOrderDetail.InvalidSku",
        "Partner product SKU cannot be empty.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProductOrderDetail.NotFound",
        $"Partner product order detail with ID '{id}' was not found.");
}
