using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;

public static class InstockInventoryError
{
    public static Error InvalidQuantity() => Error.Validation(
        "InstockInventory.InvalidQuantity",
        "Total quantity cannot be negative.");

    public static Error InsufficientQuantity(int available, int requested) => Error.Validation(
        "InstockInventory.InsufficientQuantity",
        $"Insufficient inventory. Available: {available}, Requested: {requested}.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockInventory.NotFound",
        $"Instock inventory with ID '{id}' was not found.");

    public static Error NotFoundByVariantId(Guid variantId) => Error.NotFound(
        "InstockInventory.NotFoundByVariantId",
        $"Instock inventory for variant ID '{variantId}' was not found.");
}
