using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockProductPriceDetailReplica : Entity<Guid>
{
    public Guid InStockPriceId { get; private set; }
    public Guid InStockProductVariantId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InStockProductPriceDetailReplica() : base()
    {
    }

    public static InStockProductPriceDetailReplica Create(
        Guid id,
        Guid inStockPriceId,
        Guid inStockProductVariantId,
        decimal unitPrice,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new InStockProductPriceDetailReplica
        {
            Id = id,
            InStockPriceId = inStockPriceId,
            InStockProductVariantId = inStockProductVariantId,
            UnitPrice = unitPrice,
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
}
