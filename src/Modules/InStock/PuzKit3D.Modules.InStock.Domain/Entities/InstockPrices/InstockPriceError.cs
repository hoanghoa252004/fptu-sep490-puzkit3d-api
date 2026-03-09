using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;

public static class InstockPriceError
{
    public static Error InvalidName() => Error.Validation(
        "InstockPrice.InvalidName",
        "Price name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "InstockPrice.NameTooLong",
        $"Price name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidDateRange() => Error.Validation(
        "InstockPrice.InvalidDateRange",
        "Effective from date must be before effective to date.");

    public static Error InvalidPriority() => Error.Validation(
        "InstockPrice.InvalidPriority",
        "Priority must be greater than zero.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockPrice.NotFound",
        $"Instock price with ID '{id}' was not found.");

    public static Error OverlappingPrices() => Error.Conflict(
        "InstockPrice.OverlappingPrices",
        "Price periods cannot overlap with existing prices.");
}
