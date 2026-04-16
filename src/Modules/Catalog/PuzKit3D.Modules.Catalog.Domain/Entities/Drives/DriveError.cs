using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

public static class DriveError
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Drive.NotFound", 
        $"Drive with ID {id} not found");

    internal static Error InvalidName() => Error.Validation(
        "Drive.InvalidName", 
        "Drive name cannot be empty or whitespace");

    internal static Error InvalidQuantity() => Error.Validation(
        "Drive.InvalidQuantity",
        "Quantity must be greater than 0");

    internal static Error InsufficientQuantity(int available, int requested) => Error.Validation(
        "Drive.InsufficientQuantity",
        $"Insufficient quantity. Available: {available}, Requested: {requested}");
}

