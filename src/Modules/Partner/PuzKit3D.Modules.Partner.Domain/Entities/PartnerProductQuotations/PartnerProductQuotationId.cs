using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

public sealed class PartnerProductQuotationId : StronglyTypedId<Guid>
{
    private PartnerProductQuotationId(Guid value) : base(value) { }

    public static PartnerProductQuotationId Create() => new(Guid.NewGuid());

    public static PartnerProductQuotationId From(Guid value) => new(value);
}
