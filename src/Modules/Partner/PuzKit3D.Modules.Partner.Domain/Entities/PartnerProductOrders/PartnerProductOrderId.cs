using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public sealed class PartnerProductOrderId : StronglyTypedId<Guid>
{
    private PartnerProductOrderId(Guid value) : base(value) { }

    public static PartnerProductOrderId Create() => new(Guid.NewGuid());

    public static PartnerProductOrderId From(Guid value) => new(value);
}
