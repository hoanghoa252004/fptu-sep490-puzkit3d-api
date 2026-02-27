using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

public static class TopicError
{
    public static Error InvalidName() => Error.Validation(
        "Topic.InvalidName",
        "Topic name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "Topic.NameTooLong",
        $"Topic name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidSlug() => Error.Validation(
        "Topic.InvalidSlug",
        "Topic slug cannot be empty.");

    public static Error SlugTooLong(int length) => Error.Validation(
        "Topic.SlugTooLong",
        $"Topic slug is too long: {length} characters. Maximum is 30 characters.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Topic.NotFound",
        $"Topic with ID '{id}' was not found.");

    public static Error DuplicateSlug(string slug) => Error.Conflict(
        "Topic.DuplicateSlug",
        $"Topic with slug '{slug}' already exists.");

    public static Error InvalidParent(Guid parentId) => Error.Validation(
        "Topic.InvalidParent",
        $"Parent topic with ID '{parentId}' does not exist.");

    public static Error CircularReference() => Error.Validation(
        "Topic.CircularReference",
        "Cannot set a topic as its own parent or create a circular reference.");
}
