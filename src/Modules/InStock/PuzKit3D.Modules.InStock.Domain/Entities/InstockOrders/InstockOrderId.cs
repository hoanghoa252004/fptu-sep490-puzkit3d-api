using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public sealed class InstockOrderId : StronglyTypedId<Guid>
{
    private InstockOrderId(Guid value) : base(value) { }

    public static InstockOrderId Create() => new(Guid.NewGuid());

    public static InstockOrderId From(Guid value) => new(value);
}
