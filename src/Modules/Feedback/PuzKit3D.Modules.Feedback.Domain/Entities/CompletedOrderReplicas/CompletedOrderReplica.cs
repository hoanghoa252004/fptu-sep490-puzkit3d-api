using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;

public sealed class CompletedOrderReplica : Entity<Guid>
{
    public string Type { get; private set; } = null!; 
    public Guid CustomerId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? VariantId { get; private set; }

    private CompletedOrderReplica(
        Guid id,
        string type,
        Guid customerId,
        Guid productId,
        Guid? variantId) : base(id)
    {
        Type = type;
        CustomerId = customerId;
        ProductId = productId;
        VariantId = variantId;
    }

    private CompletedOrderReplica() : base()
    {
    }

    public static CompletedOrderReplica Create(
        Guid id,
        string type,
        Guid customerId,
        Guid productId,
        Guid? variantId)
    {
        return new CompletedOrderReplica(
            id,
            type,
            customerId,
            productId,
            variantId);
    }
}
