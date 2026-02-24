using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Orders;

public sealed class OrderItemId : StronglyTypedId<Guid>
{
    private OrderItemId(Guid value) : base(value) { }

    public static OrderItemId Create() => new(Guid.NewGuid());

    public static OrderItemId From(Guid value) => new(value);
}
