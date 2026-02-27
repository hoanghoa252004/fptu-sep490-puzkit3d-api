using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Materials;

public static class MaterialError
{
    public static Error InvalidName() => Error.Validation(
        "Material.InvalidName",
        "Material name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "Material.NameTooLong",
        $"Material name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidSlug() => Error.Validation(
        "Material.InvalidSlug",
        "Material slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "Material.SlugTooLong",
        $"Material slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Material.NotFound",
        $"Material with ID '{id}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "Material.DuplicateSlug",
        $"Material with slug '{slug}' already exists.");
}
