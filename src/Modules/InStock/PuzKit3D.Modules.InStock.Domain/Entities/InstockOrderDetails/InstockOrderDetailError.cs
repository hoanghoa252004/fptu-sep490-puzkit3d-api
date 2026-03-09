using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails;

public static class InstockOrderDetailError
{
    public static Error InvalidSku() => Error.Validation(
        "InstockOrderDetail.InvalidSku",
        "SKU cannot be empty.");

    public static Error InvalidQuantity() => Error.Validation(
        "InstockOrderDetail.InvalidQuantity",
        "Quantity must be greater than zero.");

    public static Error InvalidUnitPrice() => Error.Validation(
        "InstockOrderDetail.InvalidUnitPrice",
        "Unit price must be greater than zero.");

    public static Error InvalidTotalAmount() => Error.Validation(
        "InstockOrderDetail.InvalidTotalAmount",
        "Total amount must be greater than zero.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockOrderDetail.NotFound",
        $"Instock order detail with ID '{id}' was not found.");
}
