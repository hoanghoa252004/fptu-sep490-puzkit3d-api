using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

public sealed class CustomDesignRequirementId : StronglyTypedId<Guid>
{
    private CustomDesignRequirementId(Guid value) : base(value) { }

    public static CustomDesignRequirementId Create() => new(Guid.NewGuid());

    public static CustomDesignRequirementId From(Guid value) => new(value);
}
