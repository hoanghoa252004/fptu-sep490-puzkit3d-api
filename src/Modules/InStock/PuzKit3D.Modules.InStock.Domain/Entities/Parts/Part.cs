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
    public string PartType { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public InstockProductId InstockProductId { get; private set; } = null!;

    public IReadOnlyCollection<Piece> Pieces => _pieces.AsReadOnly();

    [GeneratedRegex(@"^PAR\d{4}$", RegexOptions.Compiled)]
    private static partial Regex CodeRegexPattern();

    private Part(
        PartId id,
        string name,
        string partType,
        string code,
        InstockProductId instockProductId) : base(id)
    {
        Name = name;
        PartType = partType;
        Code = code;
        InstockProductId = instockProductId;
    }

    private Part() : base()
    {
    }

    public static ResultT<Part> Create(
        string name,
        string partType,
        string code,
        InstockProductId instockProductId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Part>(PartError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<Part>(PartError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(partType))
            return Result.Failure<Part>(PartError.InvalidPartType());

        if (partType.Length > 30)
            return Result.Failure<Part>(PartError.PartTypeTooLong(partType.Length));

        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<Part>(PartError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure<Part>(PartError.InvalidCodeFormat());

        var partId = PartId.Create();
        var part = new Part(partId, name, partType, code, instockProductId);

        return Result.Success(part);
    }

    public Result Update(string name, string partType, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PartError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(PartError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(partType))
            return Result.Failure(PartError.InvalidPartType());

        if (partType.Length > 30)
            return Result.Failure(PartError.PartTypeTooLong(partType.Length));

        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure(PartError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure(PartError.InvalidCodeFormat());

        Name = name;
        PartType = partType;
        Code = code;

        return Result.Success();
    }

    public Result PartialUpdate(string? name = null, string? partType = null)
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
            if (string.IsNullOrWhiteSpace(partType))
                return Result.Failure(PartError.InvalidPartType());

            if (partType.Length > 30)
                return Result.Failure(PartError.PartTypeTooLong(partType.Length));

            PartType = partType;
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

