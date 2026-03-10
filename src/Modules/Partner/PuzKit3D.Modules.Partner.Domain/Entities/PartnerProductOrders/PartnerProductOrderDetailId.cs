using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public sealed class PartnerProductOrderDetailId : StronglyTypedId<Guid>
{
    private PartnerProductOrderDetailId(Guid value) : base(value) { }

    public static PartnerProductOrderDetailId Create() => new(Guid.NewGuid());

    public static PartnerProductOrderDetailId From(Guid value) => new(value);
}
