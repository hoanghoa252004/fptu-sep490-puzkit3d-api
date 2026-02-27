using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;

public sealed class CapabilityId : StronglyTypedId<Guid>
{
    private CapabilityId(Guid value) : base(value) { }

    public static CapabilityId Create() => new(Guid.NewGuid());

    public static CapabilityId From(Guid value) => new(value);
}
