using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockInventoryReplica : Entity<Guid>
{
    public Guid InStockProductVariantId { get; private set; }
    public int TotalQuantity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InStockInventoryReplica() : base()
    {
    }

    public static InStockInventoryReplica Create(
        Guid id,
        Guid inStockProductVariantId,
        int totalQuantity,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new InStockInventoryReplica
        {
            Id = id,
            InStockProductVariantId = inStockProductVariantId,
            TotalQuantity = totalQuantity,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public void UpdateQuantity(int quantity)
    {
        TotalQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}
