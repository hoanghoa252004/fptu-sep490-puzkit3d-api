using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;

public sealed class RequirementCapabilityDetailId : StronglyTypedId<Guid>
{
    private RequirementCapabilityDetailId(Guid value) : base(value) { }

    public static RequirementCapabilityDetailId Create() => new(Guid.NewGuid());

    public static RequirementCapabilityDetailId From(Guid value) => new(value);
}
