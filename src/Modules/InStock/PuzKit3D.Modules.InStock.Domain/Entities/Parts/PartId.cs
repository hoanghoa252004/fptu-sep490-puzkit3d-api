using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Parts;

public sealed class PartId : StronglyTypedId<Guid>
{
    private PartId(Guid value) : base(value) { }

    public static PartId Create() => new(Guid.NewGuid());

    public static PartId From(Guid value) => new(value);
}
