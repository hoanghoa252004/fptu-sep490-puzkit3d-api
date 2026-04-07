using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;

public static class ProductQuotationError
{
    public static Error InvalidCode() => Error.Validation(
        "ProductQuotation.InvalidCode",
        "Product quotation code cannot be empty.");

    public static Error InvalidVolume() => Error.Validation(
        "ProductQuotation.InvalidVolume",
        "Product quotation volume must be greater than 0.");

    public static Error InvalidMaterialId() => Error.Validation(
        "ProductQuotation.InvalidMaterialId",
        "Product quotation material ID cannot be empty.");

    public static Error InvalidMaterialBasePrice() => Error.Validation(
        "ProductQuotation.InvalidMaterialBasePrice",
        "Product quotation material base price cannot be negative.");

    public static Error InvalidBaseAmount() => Error.Validation(
        "ProductQuotation.InvalidBaseAmount",
        "Product quotation base amount cannot be negative.");

    public static Error InvalidWeightPercent() => Error.Validation(
        "ProductQuotation.InvalidWeightPercent",
        "Product quotation weight percent must be between 0 and 100.");

    public static Error InvalidWeightAmount() => Error.Validation(
        "ProductQuotation.InvalidWeightAmount",
        "Product quotation weight amount cannot be negative.");

    public static Error InvalidTotalAmount() => Error.Validation(
        "ProductQuotation.InvalidTotalAmount",
        "Product quotation total amount cannot be negative.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "ProductQuotation.NotFound",
        $"Product quotation with ID '{id}' was not found.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "ProductQuotation.DuplicateCode",
        $"Product quotation with code '{code}' already exists.");
}
