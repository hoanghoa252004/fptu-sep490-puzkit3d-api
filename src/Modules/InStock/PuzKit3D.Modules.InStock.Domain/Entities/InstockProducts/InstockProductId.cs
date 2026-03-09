using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

public sealed class InstockProductId : StronglyTypedId<Guid>
{
    private InstockProductId(Guid value) : base(value) { }

    public static InstockProductId Create() => new(Guid.NewGuid());

    public static InstockProductId From(Guid value) => new(value);
}
