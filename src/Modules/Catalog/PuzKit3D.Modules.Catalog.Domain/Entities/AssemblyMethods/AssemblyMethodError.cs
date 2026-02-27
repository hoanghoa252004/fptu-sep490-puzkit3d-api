using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

public static class AssemblyMethodError
{
    public static Error InvalidName() => Error.Validation(
        "AssemblyMethod.InvalidName",
        "Assembly method name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "AssemblyMethod.NameTooLong",
        $"Assembly method name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidSlug() => Error.Validation(
        "AssemblyMethod.InvalidSlug",
        "Assembly method slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "AssemblyMethod.SlugTooLong",
        $"Assembly method slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "AssemblyMethod.NotFound",
        $"Assembly method with ID '{id}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "AssemblyMethod.DuplicateSlug",
        $"Assembly method with slug '{slug}' already exists.");
}
