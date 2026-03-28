using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;

public static class InstockProductPriceDetailError
{
    public static Error InvalidUnitPrice() => Error.Validation(
        "InstockProductPriceDetail.InvalidUnitPrice",
        "Unit price must be at least 10,000.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockProductPriceDetail.NotFound",
        $"Instock product price detail with ID '{id}' was not found.");

    public static Error DuplicatePriceDetail() => Error.Conflict(
        "InstockProductPriceDetail.DuplicatePriceDetail",
        "Price detail for this product variant and price already exists.");

    public static Error AlreadyActive(Guid id) => Error.Conflict(
        "InstockProductPriceDetail.AlreadyActive",
        $"Instock product price detail with ID '{id}' is already active.");

    public static Error AlreadyInactive(Guid id) => Error.Conflict(
        "InstockProductPriceDetail.AlreadyInactive",
        $"Instock product price detail with ID '{id}' is already inactive.");

    public static Error IsActiveUnchanged(bool currentValue) => Error.Validation(
        "InstockProductPriceDetail.IsActiveUnchanged",
        $"Price detail is already {(currentValue ? "active" : "inactive")}. No change needed.");

    public static Error CannotDeleteWithOrders(Guid id) => Error.Conflict(
        "InstockProductPriceDetail.CannotDeleteWithOrders",
        $"Cannot delete price detail with ID '{id}' because it has been used in existing orders. Please deactivate the price detail instead.");
}

