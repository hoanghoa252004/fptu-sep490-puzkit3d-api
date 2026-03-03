using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public sealed class CartItemId : StronglyTypedId<Guid>
{
    private CartItemId(Guid value) : base(value) { }

    public static CartItemId Create() => new(Guid.NewGuid());

    public static CartItemId From(Guid value) => new(value);
}
