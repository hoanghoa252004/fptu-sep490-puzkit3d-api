using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public sealed class CartTypeId : StronglyTypedId<Guid>
{
    private CartTypeId(Guid value) : base(value) { }

    public static CartTypeId Create() => new(Guid.NewGuid());

    public static CartTypeId From(Guid value) => new(value);
}
