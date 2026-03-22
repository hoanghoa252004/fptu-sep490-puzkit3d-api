using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Pieces;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.RegularExpressions;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Parts;

public sealed partial class Part : Entity<PartId>
{
    private static readonly Regex CodeRegex = CodeRegexPattern();
    private readonly List<Piece> _pieces = new();

    public string Name { get; private set; } = null!;
    public PartType PartType { get; private set; }
    public string Code { get; private set; } = null!;
    public int Quantity { get; private set; }
    public InstockProductId InstockProductId { get; private set; } = null!;

    public IReadOnlyCollection<Piece> Pieces => _pieces.AsReadOnly();

    [GeneratedRegex(@"^PAR\d{4}$", RegexOptions.Compiled)]
    private static partial Regex CodeRegexPattern();

    private Part(
        PartId id,
        string name,
        PartType partType,
        string code,
        int quantity,
        InstockProductId instockProductId) : base(id)
    {
        Name = name;
        PartType = partType;
        Code = code;
        Quantity = quantity;
        InstockProductId = instockProductId;
    }

    private Part() : base()
    {
    }

    public static ResultT<Part> Create(
        string name,
        PartType partType,
        string code,
        int quantity,
        InstockProductId instockProductId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Part>(PartError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<Part>(PartError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<Part>(PartError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure<Part>(PartError.InvalidCodeFormat());

        if (quantity <= 0)
            return Result.Failure<Part>(PartError.InvalidQuantity());

        var partId = PartId.Create();
        var part = new Part(partId, name, partType, code, quantity, instockProductId);

        return Result.Success(part);
    }

    public Result Update(string name, PartType partType, string code, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PartError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(PartError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure(PartError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure(PartError.InvalidCodeFormat());

        if (quantity <= 0)
            return Result.Failure(PartError.InvalidQuantity());

        Name = name;
        PartType = partType;
        Code = code;
        Quantity = quantity;

        return Result.Success();
    }

    public Result PartialUpdate(string? name = null, PartType? partType = null, int? quantity = null)
    {
        if (name is not null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(PartError.InvalidName());

            if (name.Length > 30)
                return Result.Failure(PartError.NameTooLong(name.Length));

            Name = name;
        }

        if (partType is not null)
        {
            PartType = partType.Value;
        }

        if (quantity is not null)
        {
            if (quantity.Value <= 0)
                return Result.Failure(PartError.InvalidQuantity());

            Quantity = quantity.Value;
        }

        return Result.Success();
    }

    public void Delete()
    {
        // Hard delete - no domain event needed as we're removing the aggregate
    }

    public void AddPiece(Piece piece)
    {
        _pieces.Add(piece);
    }

    public void RemovePiece(Piece piece)
    {
        _pieces.Remove(piece);
    }
}

