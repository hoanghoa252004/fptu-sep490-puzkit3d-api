using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Orders;

public sealed class OrderId : StronglyTypedId<Guid>
{
    private OrderId(Guid value) : base(value) { }

    public static OrderId Create() => new(Guid.NewGuid());

    public static OrderId From(Guid value) => new(value);
}
