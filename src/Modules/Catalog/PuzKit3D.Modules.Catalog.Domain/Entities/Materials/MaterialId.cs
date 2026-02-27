using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Materials;

public sealed class MaterialId : StronglyTypedId<Guid>
{
    private MaterialId(Guid value) : base(value) { }

    public static MaterialId Create() => new(Guid.NewGuid());

    public static MaterialId From(Guid value) => new(value);
}
