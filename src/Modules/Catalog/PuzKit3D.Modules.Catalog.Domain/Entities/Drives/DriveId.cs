using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

public sealed class DriveId : StronglyTypedId<Guid>
{
    private DriveId(Guid value) : base(value) { }

    public static DriveId Create() => new(Guid.NewGuid());

    public static DriveId From(Guid value) => new(value);
}
