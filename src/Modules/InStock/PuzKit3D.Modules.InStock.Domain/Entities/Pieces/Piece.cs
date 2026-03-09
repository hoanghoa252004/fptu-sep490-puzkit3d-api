using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.RegularExpressions;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Pieces;

public sealed partial class Piece : Entity<PieceId>
{
    private static readonly Regex CodeRegex = CodeRegexPattern();

    public string Code { get; private set; } = null!;
    public int Quantity { get; private set; }
    public PartId PartId { get; private set; } = null!;

    [GeneratedRegex(@"^PIE\d{5}$", RegexOptions.Compiled)]
    private static partial Regex CodeRegexPattern();

    private Piece(
        PieceId id,
        string code,
        int quantity,
        PartId partId) : base(id)
    {
        Code = code;
        Quantity = quantity;
        PartId = partId;
    }

    private Piece() : base()
    {
    }

    public static ResultT<Piece> Create(
        string code,
        int quantity,
        PartId partId)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<Piece>(PieceError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure<Piece>(PieceError.InvalidCodeFormat());

        if (code.Length > 10)
            return Result.Failure<Piece>(PieceError.CodeTooLong(code.Length));

        if (quantity <= 0)
            return Result.Failure<Piece>(PieceError.InvalidQuantity());

        var pieceId = PieceId.Create();
        var piece = new Piece(pieceId, code, quantity, partId);

        return Result.Success(piece);
    }

    public Result Update(string code, int quantity)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure(PieceError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure(PieceError.InvalidCodeFormat());

        if (code.Length > 10)
            return Result.Failure(PieceError.CodeTooLong(code.Length));

        if (quantity <= 0)
            return Result.Failure(PieceError.InvalidQuantity());

        Code = code;
        Quantity = quantity;

        return Result.Success();
    }
}

