using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;

public static class InstockProductVariantError
{
    public static Error InvalidSku() => Error.Validation(
        "InstockProductVariant.InvalidSku",
        "SKU cannot be empty.");

    public static Error SkuTooLong(int length) => Error.Validation(
        "InstockProductVariant.SkuTooLong",
        $"SKU is too long: {length} characters. Maximum is 10 characters.");

    public static Error InvalidColor() => Error.Validation(
        "InstockProductVariant.InvalidColor",
        "Color cannot be empty.");

    public static Error ColorTooLong(int length) => Error.Validation(
        "InstockProductVariant.ColorTooLong",
        $"Color is too long: {length} characters. Maximum is 15 characters.");

    public static Error InvalidDimension() => Error.Validation(
        "InstockProductVariant.InvalidDimension",
        "All dimensions must be greater than zero.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockProductVariant.NotFound",
        $"Instock product variant with ID '{id}' was not found.");

    public static Error DuplicateSku(string sku) => Error.Conflict(
        "InstockProductVariant.DuplicateSku",
        $"Instock product variant with SKU '{sku}' already exists.");

    public static Error AlreadyActive(Guid id) => Error.Conflict(
        "InstockProductVariant.AlreadyActive",
        $"Instock product variant with ID '{id}' is already active.");

    public static Error AlreadyInactive(Guid id) => Error.Conflict(
        "InstockProductVariant.AlreadyInactive",
        $"Instock product variant with ID '{id}' is already inactive.");

    public static Error VariantDoesNotBelongToProduct(Guid variantId, Guid productId) => Error.Validation(
        "InstockProductVariant.VariantDoesNotBelongToProduct",
        $"Variant with ID '{variantId}' does not belong to product with ID '{productId}'.");
}
