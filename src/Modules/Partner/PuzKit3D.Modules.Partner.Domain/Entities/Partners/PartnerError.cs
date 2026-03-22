using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.Partners;

public static class PartnerError
{
    public static Error InvalidName() => Error.Validation(
        "Partner.InvalidName",
        "Partner name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "Partner.NameTooLong",
        $"Partner name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidSlug() => Error.Validation(
        "Partner.InvalidSlug",
        "Partner slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "Partner.SlugTooLong",
        $"Partner slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Partner.NotFound",
        $"Partner with ID '{id}' was not found.");

    public static Error NotFoundBySlug(string slug) => Error.NotFound(
        "Partner.NotFoundBySlug",
        $"Partner with slug '{slug}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "Partner.DuplicateSlug",
        $"Partner with slug '{slug}' already exists.");

    public static Error AlreadyInactive(Guid id) => Error.Conflict(
        "Partner.AlreadyInactive",
        $"Partner with ID '{id}' is already inactive.");

    public static Error AlreadyActive(Guid id) => Error.Conflict(
        "Partner.AlreadyActive",
        $"Partner with ID '{id}' is already active.");
}
