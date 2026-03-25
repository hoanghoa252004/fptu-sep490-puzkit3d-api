using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.Partners;

public static class PartnerError
{
    // Name
    public static Error EmptyName() => Error.Validation(
        "Partner.EmptyName",
        "Partner name cannot be empty.");

    public static Error InvalidName() => Error.Validation(
        "Partner.InvalidName",
        "Partner name is invalid.");

    public static Error NameTooLong(int length) => Error.Validation(
        "Partner.NameTooLong",
        $"Partner name is too long: {length} characters. Maximum is 30 characters.");

    // Slug
    public static Error EmptySlug() => Error.Validation(
        "Partner.EmptySlug",
        "Partner slug cannot be empty.");

    public static Error InvalidSlug() => Error.Validation(
        "Partner.InvalidSlug",
        "Partner slug is invalid.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "Partner.SlugTooLong",
        $"Partner slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "Partner.DuplicateSlug",
        $"Partner with slug '{slug}' already exists.");

    // Partner
    public static Error NotFound(Guid id) => Error.NotFound(
        "Partner.NotFound",
        $"Partner with ID '{id}' was not found.");

    public static Error NotFoundBySlug(string slug) => Error.NotFound(
        "Partner.NotFoundBySlug",
        $"Partner with slug '{slug}' was not found.");

    public static Error AlreadyInactive(Guid id) => Error.Conflict(
        "Partner.AlreadyInactive",
        $"Partner with ID '{id}' is already inactive.");

    public static Error AlreadyActive(Guid id) => Error.Conflict(
        "Partner.AlreadyActive",
        $"Partner with ID '{id}' is already active.");

    // Import service configuration
    public static Error NotFoundImportServiceConfig() => Error.Validation(
        "Partner.NotFoundImportServiceConfig",
        "Import service configuration not found.");
    
    public static Error EmptyImportServiceConfig() => Error.Validation(
        "Partner.EmptyImportServiceConfig",
        "Import service configuration cannot be empty.");

    // Address

    public static Error EmptyAddress() => Error.Validation(
        "Partner.EmptyAddress",
        "Address cannot be empty.");

    // Email
    public static Error EmptyEmail() => Error.Validation(
        "Partner.EmptyEmail",
        "Email cannot be empty.");

    public static Error InvalidEmail() => Error.Validation(
        "Partner.InvalidEmail",
        "Invalid email.");

    // Phone
    public static Error EmptyPhone() => Error.Validation(
        "Partner.EmptyPhone",
        "Phone number cannot be empty.");

    public static Error InvalidPhone() => Error.Validation(
        "Partner.InvalidPhone",
        "Invalid phone.");
}
