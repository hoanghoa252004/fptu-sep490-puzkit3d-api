using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

public sealed class MilestoneId : StronglyTypedId<Guid>
{
    private MilestoneId(Guid value) : base(value) { }

    public static MilestoneId Create() => new(Guid.NewGuid());

    public static MilestoneId From(Guid value) => new(value);
}
