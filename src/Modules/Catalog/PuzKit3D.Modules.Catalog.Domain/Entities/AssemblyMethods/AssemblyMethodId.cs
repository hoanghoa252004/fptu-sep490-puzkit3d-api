using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

public sealed class AssemblyMethodId : StronglyTypedId<Guid>
{
    private AssemblyMethodId(Guid value) : base(value) { }

    public static AssemblyMethodId Create() => new(Guid.NewGuid());

    public static AssemblyMethodId From(Guid value) => new(value);
}
