using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;

public static class InstockProductPriceDetailError
{
    public static Error InvalidUnitPrice() => Error.Validation(
        "InstockProductPriceDetail.InvalidUnitPrice",
        "Unit price must be greater than zero.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockProductPriceDetail.NotFound",
        $"Instock product price detail with ID '{id}' was not found.");

    public static Error DuplicatePriceDetail() => Error.Conflict(
        "InstockProductPriceDetail.DuplicatePriceDetail",
        "Price detail for this product variant and price already exists.");
}
