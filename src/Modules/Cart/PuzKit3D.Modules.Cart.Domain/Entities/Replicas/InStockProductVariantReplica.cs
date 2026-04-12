using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockProductVariantReplica : Entity<Guid>
{
    public Guid InStockProductId { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public int AssembledLengthMm { get; private set; }
    public int AssembledWidthMm { get; private set; }
    public int AssembledHeightMm { get; private set; }
    public string PreviewImages { get; private set; } = null!;
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
        int assembledLengthMm,
        int assembledWidthMm,
        int assembledHeightMm,
        string previewImages,
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
            AssembledLengthMm = assembledLengthMm,
            AssembledWidthMm = assembledWidthMm,
            AssembledHeightMm = assembledHeightMm,
            PreviewImages = previewImages,
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
