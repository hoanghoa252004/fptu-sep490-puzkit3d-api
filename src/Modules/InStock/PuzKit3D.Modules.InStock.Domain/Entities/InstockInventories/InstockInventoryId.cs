using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;

public sealed class InstockInventoryId : StronglyTypedId<Guid>
{
    private InstockInventoryId(Guid value) : base(value) { }

    public static InstockInventoryId Create() => new(Guid.NewGuid());

    public static InstockInventoryId From(Guid value) => new(value);
}
