using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;

public sealed class PartnerProductQuotationDetailId : StronglyTypedId<Guid>
{
    private PartnerProductQuotationDetailId(Guid value) : base(value) { }

    public static PartnerProductQuotationDetailId Create() => new(Guid.NewGuid());

    public static PartnerProductQuotationDetailId From(Guid value) => new(value);
}
