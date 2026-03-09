using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Pieces;

public static class PieceError
{
    public static Error InvalidCode() => Error.Validation(
        "Piece.InvalidCode",
        "Piece code cannot be empty.");

    public static Error InvalidCodeFormat() => Error.Validation(
        "Piece.InvalidCodeFormat",
        "Piece code must be in format PIExxxxx (e.g., PIE00001, PIE00002).");

    public static Error CodeTooLong(int length) => Error.Validation(
        "Piece.CodeTooLong",
        $"Piece code is too long: {length} characters. Maximum is 10 characters.");

    public static Error InvalidQuantity() => Error.Validation(
        "Piece.InvalidQuantity",
        "Quantity must be greater than zero.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Piece.NotFound",
        $"Piece with ID '{id}' was not found.");
}

