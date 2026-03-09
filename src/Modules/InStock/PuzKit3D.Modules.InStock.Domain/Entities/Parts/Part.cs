using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Pieces;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Parts;

public sealed class Part : Entity<PartId>
{
    private readonly List<Piece> _pieces = new();

    public string Name { get; private set; } = null!;
    public string PartType { get; private set; } = null!;
    public InstockProductId InstockProductId { get; private set; } = null!;

    public IReadOnlyCollection<Piece> Pieces => _pieces.AsReadOnly();

    private Part(
        PartId id,
        string name,
        string partType,
        InstockProductId instockProductId) : base(id)
    {
        Name = name;
        PartType = partType;
        InstockProductId = instockProductId;
    }

    private Part() : base()
    {
    }

    public static ResultT<Part> Create(
        string name,
        string partType,
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

        var partId = PartId.Create();
        var part = new Part(partId, name, partType, instockProductId);

        return Result.Success(part);
    }

    public Result Update(string name, string partType)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PartError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(PartError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(partType))
            return Result.Failure(PartError.InvalidPartType());

        if (partType.Length > 30)
            return Result.Failure(PartError.PartTypeTooLong(partType.Length));

        Name = name;
        PartType = partType;

        return Result.Success();
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
