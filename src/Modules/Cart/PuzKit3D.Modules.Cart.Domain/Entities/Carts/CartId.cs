using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public sealed class CartId : StronglyTypedId<Guid>
{
    private CartId(Guid value) : base(value) { }

    public static CartId Create() => new(Guid.NewGuid());

    public static CartId From(Guid value) => new(value);
}
