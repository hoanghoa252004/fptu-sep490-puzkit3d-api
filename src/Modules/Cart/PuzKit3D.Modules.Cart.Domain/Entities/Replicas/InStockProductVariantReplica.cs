using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockProductVariantReplica : Entity<Guid>
{
    public Guid InStockProductId { get; private set; }
    public string Sku { get; private set; }
    public string Color { get; private set; }
    public string Size { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InStockProductVariantReplica() : base()
    {
    }

    public static InStockProductVariantReplica Create(
        Guid id,
        Guid inStockProductId,
        string sku,
        string color,
        string size,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new InStockProductVariantReplica
        {
            Id = id,
            InStockProductId = inStockProductId,
            Sku = sku,
            Color = color,
            Size = size,
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
}
