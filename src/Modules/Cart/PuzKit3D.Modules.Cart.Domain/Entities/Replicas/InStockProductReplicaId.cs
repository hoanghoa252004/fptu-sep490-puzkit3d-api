using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockProductReplicaId : StronglyTypedId<Guid>
{
    private InStockProductReplicaId(Guid value) : base(value) { }

    public static InStockProductReplicaId Create() => new(Guid.NewGuid());

    public static InStockProductReplicaId From(Guid value) => new(value);
}
