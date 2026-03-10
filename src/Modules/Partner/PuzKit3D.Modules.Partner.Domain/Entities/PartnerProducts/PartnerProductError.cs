using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;

public static class PartnerProductError
{
    public static Error InvalidName() => Error.Validation(
        "PartnerProduct.InvalidName",
        "Partner product name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "PartnerProduct.NameTooLong",
        $"Partner product name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidReferencePrice() => Error.Validation(
        "PartnerProduct.InvalidReferencePrice",
        "Reference price must be greater than or equal to 0.");

    public static Error InvalidSlug() => Error.Validation(
        "PartnerProduct.InvalidSlug",
        "Partner product slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "PartnerProduct.SlugTooLong",
        $"Partner product slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProduct.NotFound",
        $"Partner product with ID '{id}' was not found.");

    public static Error NotFoundBySlug(string slug) => Error.NotFound(
        "PartnerProduct.NotFoundBySlug",
        $"Partner product with slug '{slug}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "PartnerProduct.DuplicateSlug",
        $"Partner product with slug '{slug}' already exists for this partner.");
}
