using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails;

public sealed class InstockOrderDetailId : StronglyTypedId<Guid>
{
    private InstockOrderDetailId(Guid value) : base(value) { }

    public static InstockOrderDetailId Create() => new(Guid.NewGuid());

    public static InstockOrderDetailId From(Guid value) => new(value);
}
