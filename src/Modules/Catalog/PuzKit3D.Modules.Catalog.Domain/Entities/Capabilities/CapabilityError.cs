using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;

public static class CapabilityError
{
    public static Error InvalidName() => Error.Validation(
        "Capability.InvalidName",
        "Capability name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "Capability.NameTooLong",
        $"Capability name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidSlug() => Error.Validation(
        "Capability.InvalidSlug",
        "Capability slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "Capability.SlugTooLong",
        $"Capability slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Capability.NotFound",
        $"Capability with ID '{id}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "Capability.DuplicateSlug",
        $"Capability with slug '{slug}' already exists.");
}
