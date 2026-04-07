using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;

public sealed class MilestoneQuotationDetailId : StronglyTypedId<Guid>
{
    private MilestoneQuotationDetailId(Guid value) : base(value) { }

    public static MilestoneQuotationDetailId Create() => new(Guid.NewGuid());

    public static MilestoneQuotationDetailId From(Guid value) => new(value);
}
