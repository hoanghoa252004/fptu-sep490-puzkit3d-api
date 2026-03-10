using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;

public sealed class ImportServiceConfigId : StronglyTypedId<Guid>
{
    private ImportServiceConfigId(Guid value) : base(value) { }

    public static ImportServiceConfigId Create() => new(Guid.NewGuid());

    public static ImportServiceConfigId From(Guid value) => new(value);
}
