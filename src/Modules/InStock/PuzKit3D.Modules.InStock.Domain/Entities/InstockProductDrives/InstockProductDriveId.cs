using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;

public sealed class InstockProductDriveId : StronglyTypedId<Guid>
{
    private InstockProductDriveId(Guid value) : base(value)
    {
    }

    public static InstockProductDriveId From(Guid value) => new(value);

    public static InstockProductDriveId Create() => new(Guid.NewGuid());
}
