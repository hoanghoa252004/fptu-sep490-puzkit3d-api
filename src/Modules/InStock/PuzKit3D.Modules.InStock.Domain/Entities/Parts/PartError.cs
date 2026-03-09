using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Parts;

public static class PartError
{
    public static Error InvalidName() => Error.Validation(
        "Part.InvalidName",
        "Part name cannot be empty.");

    public static Error NameTooLong(int length) => Error.Validation(
        "Part.NameTooLong",
        $"Part name is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidPartType() => Error.Validation(
        "Part.InvalidPartType",
        "Part type cannot be empty.");

    public static Error PartTypeTooLong(int length) => Error.Validation(
        "Part.PartTypeTooLong",
        $"Part type is too long: {length} characters. Maximum is 30 characters.");

    public static Error InvalidCode() => Error.Validation(
        "Part.InvalidCode",
        "Part code cannot be empty.");

    public static Error InvalidCodeFormat() => Error.Validation(
        "Part.InvalidCodeFormat",
        "Part code must be in format PARxxxx (e.g., PAR0001, PAR0002).");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Part.NotFound",
        $"Part with ID '{id}' was not found.");
}

