using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;

public sealed class MilestoneQuotationId : StronglyTypedId<Guid>
{
    private MilestoneQuotationId(Guid value) : base(value) { }

    public static MilestoneQuotationId Create() => new(Guid.NewGuid());

    public static MilestoneQuotationId From(Guid value) => new(value);
}
