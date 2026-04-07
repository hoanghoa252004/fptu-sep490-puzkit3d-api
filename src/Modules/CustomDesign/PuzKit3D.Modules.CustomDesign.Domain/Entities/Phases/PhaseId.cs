using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;

public sealed class PhaseId : StronglyTypedId<Guid>
{
    private PhaseId(Guid value) : base(value) { }

    public static PhaseId Create() => new(Guid.NewGuid());

    public static PhaseId From(Guid value) => new(value);
}
