using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderItemReplicas;

public sealed class CompletedOrderItemReplica : Entity<Guid>
{
    public Guid CompletedOrderReplicaId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? VariantId { get; private set; }
    public int Quantity { get; private set; }

    private CompletedOrderItemReplica(
        Guid id,
        Guid completedOrderReplicaId,
        Guid productId,
        Guid? variantId,
        int quantity) : base(id)
    {
        CompletedOrderReplicaId = completedOrderReplicaId;
        ProductId = productId;
        VariantId = variantId;
        Quantity = quantity;
    }

    private CompletedOrderItemReplica() : base()
    {
    }

    public static CompletedOrderItemReplica Create(
        Guid id,
        Guid completedOrderReplicaId,
        Guid productId,
        Guid? variantId,
        int quantity)
    {
        return new CompletedOrderItemReplica(
            id,
            completedOrderReplicaId,
            productId,
            variantId,
            quantity);
    }
}
