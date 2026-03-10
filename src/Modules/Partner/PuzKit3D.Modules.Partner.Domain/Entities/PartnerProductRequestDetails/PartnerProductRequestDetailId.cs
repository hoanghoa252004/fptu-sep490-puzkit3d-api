using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;

public sealed class PartnerProductRequestDetailId : StronglyTypedId<Guid>
{
    private PartnerProductRequestDetailId(Guid value) : base(value) { }

    public static PartnerProductRequestDetailId Create() => new(Guid.NewGuid());

    public static PartnerProductRequestDetailId From(Guid value) => new(value);
}
