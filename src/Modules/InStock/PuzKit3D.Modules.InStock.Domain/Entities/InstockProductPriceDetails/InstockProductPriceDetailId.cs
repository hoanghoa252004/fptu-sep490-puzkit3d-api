using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;

public sealed class InstockProductPriceDetailId : StronglyTypedId<Guid>
{
    private InstockProductPriceDetailId(Guid value) : base(value) { }

    public static InstockProductPriceDetailId Create() => new(Guid.NewGuid());

    public static InstockProductPriceDetailId From(Guid value) => new(value);
}
