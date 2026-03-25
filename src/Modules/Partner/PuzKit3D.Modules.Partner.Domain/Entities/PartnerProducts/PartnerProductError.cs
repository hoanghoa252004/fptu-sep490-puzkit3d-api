using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;

public static class PartnerProductError
{
    // Name
    public static Error EmptyName() => Error.Validation(
        "PartnerProduct.EmptyName",
        "Partner product name cannot be empty.");

    public static Error InvalidName() => Error.Validation(
        "PartnerProduct.InvalidName",
        "Partner product name is invalid.");
    public static Error NameTooLong(int length) => Error.Validation(
        "PartnerProduct.NameTooLong",
        $"Partner product name is too long: {length} characters. Maximum is 30 characters.");

    // Reference Price
    public static Error InvalidReferencePrice() => Error.Validation(
        "PartnerProduct.InvalidReferencePrice",
        "Reference price must be greater than or equal to 50000.");

    // Quantity
    public static Error InvalidQuantity() => Error.Validation(
        "PartnerProduct.InvalidQuantity",
        "Quantity must be greater than or equal to 0.");

    // Slug
    public static Error InvalidSlug() => Error.Validation(
        "PartnerProduct.InvalidSlug",
        "Partner product slug is invalid.");
    
    public static Error EmptySlug() => Error.Validation(
        "PartnerProduct.EmptySlug",
        "Partner product slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "PartnerProduct.SlugTooLong",
        $"Partner product slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error NotFoundBySlug(string slug) => Error.NotFound(
        "PartnerProduct.NotFoundBySlug",
        $"Partner product with slug '{slug}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "PartnerProduct.DuplicateSlug",
        $"Partner product with slug '{slug}' already exists for this partner.");

    // Partner product
    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProduct.NotFound",
        $"Partner product with ID '{id}' was not found.");

    public static Error AlreadyActive(Guid id) => Error.Validation(
        "PartnerProduct.AlreadyActive",
        $"Partner product with ID '{id}' is already active.");

    public static Error AlreadyInactive(Guid id) => Error.Validation(
        "PartnerProduct.AlreadyInactive",
        $"Partner product with ID '{id}' is already inactive.");

    // Thumbnail URL
    public static Error EmptyThumbnailUrl() => Error.Validation(
        "PartnerProduct.EmptyThumbnailUrl",
        "Thumbnail URL cannot be empty.");

    // Preview asset
    public static Error EmptyPreviewAsset() => Error.Validation(
        "PartnerProduct.EmptyPreviewAsset",
        "Preview asset cannot be empty.");
}
