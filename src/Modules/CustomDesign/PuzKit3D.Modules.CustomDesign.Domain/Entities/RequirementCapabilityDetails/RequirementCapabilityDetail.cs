using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;

public sealed class RequirementCapabilityDetail : Entity<RequirementCapabilityDetailId>
{
    public CustomDesignRequirementId CustomDesignRequirementId { get; private set; }
    public Guid CapabilityId { get; private set; }

    private RequirementCapabilityDetail(
        RequirementCapabilityDetailId id,
        CustomDesignRequirementId customDesignRequirementId,
        Guid capabilityId) : base(id)
    {
        CustomDesignRequirementId = customDesignRequirementId;
        CapabilityId = capabilityId;
    }

    private RequirementCapabilityDetail() : base()
    {
    }

    public static RequirementCapabilityDetail Create(
        Guid id,
        Guid customDesignRequirementId,
        Guid capabilityId)
    {
        return new RequirementCapabilityDetail(
            RequirementCapabilityDetailId.From(id),
            CustomDesignRequirementId.From(customDesignRequirementId),
            capabilityId);
    }
}
