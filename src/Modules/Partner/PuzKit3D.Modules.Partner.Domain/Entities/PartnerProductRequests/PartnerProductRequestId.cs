using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

public sealed class PartnerProductRequestId : StronglyTypedId<Guid>
{
    private PartnerProductRequestId(Guid value) : base(value) { }

    public static PartnerProductRequestId Create() => new(Guid.NewGuid());

    public static PartnerProductRequestId From(Guid value) => new(value);
}
