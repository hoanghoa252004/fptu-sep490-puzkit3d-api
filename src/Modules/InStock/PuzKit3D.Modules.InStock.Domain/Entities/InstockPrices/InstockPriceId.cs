using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;

public sealed class InstockPriceId : StronglyTypedId<Guid>
{
    private InstockPriceId(Guid value) : base(value) { }

    public static InstockPriceId Create() => new(Guid.NewGuid());

    public static InstockPriceId From(Guid value) => new(value);
}
