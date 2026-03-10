using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;

public sealed class PartnerProductId : StronglyTypedId<Guid>
{
    private PartnerProductId(Guid value) : base(value) { }

    public static PartnerProductId Create() => new(Guid.NewGuid());

    public static PartnerProductId From(Guid value) => new(value);
}
