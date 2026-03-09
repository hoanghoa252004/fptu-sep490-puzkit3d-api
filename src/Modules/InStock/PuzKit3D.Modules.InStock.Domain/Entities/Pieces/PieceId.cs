using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Pieces;

public sealed class PieceId : StronglyTypedId<Guid>
{
    private PieceId(Guid value) : base(value) { }

    public static PieceId Create() => new(Guid.NewGuid());

    public static PieceId From(Guid value) => new(value);
}
