using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

public static class InstockProductError
{
    public static Error InvalidCode() => Error.Validation(
        "InstockProduct.InvalidCode",
        "Product code cannot be empty.");

    public static Error InvalidCodeFormat() => Error.Validation(
        "InstockProduct.InvalidCodeFormat",
        "Product code must be in format INPxxx (e.g., INP001, INP002).");

    public static Error CodeTooLong(int length) => Error.Validation(
        "InstockProduct.CodeTooLong",
        $"Product code is too long: {length} characters. Maximum is 10 characters.");

    public static Error InvalidSlug() => Error.Validation(
        "InstockProduct.InvalidSlug",
        "Product slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "InstockProduct.SlugTooLong",
        $"Product slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidName() => Error.Validation(
        "InstockProduct.InvalidName",
        "Product name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "InstockProduct.NameTooLong",
        $"Product name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidTotalPieceCount() => Error.Validation(
        "InstockProduct.InvalidTotalPieceCount",
        "Total piece count must be greater than zero.");

    public static Error InvalidDifficultLevel() => Error.Validation(
        "InstockProduct.InvalidDifficultLevel",
        "Difficult level cannot be empty.");

    public static Error InvalidDifficultLevelValue(string value) => Error.Validation(
        "InstockProduct.InvalidDifficultLevelValue",
        $"Difficult level '{value}' is not valid. Only 'Basic', 'Intermediate', or 'Advanced' are allowed.");

    public static Error InvalidEstimatedBuildTime() => Error.Validation(
        "InstockProduct.InvalidEstimatedBuildTime",
        "Estimated build time must be greater than zero.");

    public static Error InvalidThumbnailUrl() => Error.Validation(
        "InstockProduct.InvalidThumbnailUrl",
        "Thumbnail URL cannot be empty.");

    public static Error InvalidPreviewAsset() => Error.Validation(
        "InstockProduct.InvalidPreviewAsset",
        "Preview asset cannot be empty.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockProduct.NotFound",
        $"Instock product with ID '{id}' was not found.");

    public static Error NotFoundBySlug(string slug) => Error.NotFound(
        "InstockProduct.NotFoundBySlug",
        $"Instock product with slug '{slug}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "InstockProduct.DuplicateSlug",
        $"Instock product with slug '{slug}' already exists.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "InstockProduct.DuplicateCode",
        $"Instock product with code '{code}' already exists.");

    public static Error AlreadyActive(Guid id) => Error.Validation(
        "InstockProduct.AlreadyActive",
        $"Instock product with ID '{id}' is already active.");

    public static Error AlreadyInactive(Guid id) => Error.Validation(
        "InstockProduct.AlreadyInactive",
        $"Instock product with ID '{id}' is already inactive.");

    public static Error IsActiveUnchanged(bool currentValue) => Error.Validation(
        "InstockProduct.IsActiveUnchanged",
        $"Product is already {(currentValue ? "active" : "inactive")}. No change needed.");
}

