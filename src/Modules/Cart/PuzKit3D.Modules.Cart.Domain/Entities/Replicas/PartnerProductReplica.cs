using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class PartnerProductReplica : Entity<Guid>
{
    public Guid PartnerId { get; private set; }
    public string Name { get; private set; }
    public decimal ReferencePrice { get; private set; }
    public int Quantity { get; private set; }
    public string? Description { get; private set; }
    public string ThumbnailUrl { get; private set; }
    public string PreviewAsset { get; private set; }
    public string Slug { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private PartnerProductReplica() : base()
    {
    }

    public static PartnerProductReplica Create(
        Guid id,
        Guid partnerId,
        string name,
        decimal referencePrice,
        int quantity,
        string? description,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        bool isActive,
        DateTime createdAt)
    {
        return new PartnerProductReplica
        {
            Id = id,
            PartnerId = partnerId,
            Name = name,
            ReferencePrice = referencePrice,
            Quantity = quantity,
            Description = description,
            ThumbnailUrl = thumbnailUrl,
            PreviewAsset = previewAsset,
            Slug = slug,
            IsActive = isActive,
            CreatedAt = createdAt
        };
    }

    public void Update(
        Guid partnerId,
        string name,
        decimal referencePrice,
        int quantity,
        string? description,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        DateTime updatedAt)
    {
        PartnerId = partnerId;
        Name = name;
        ReferencePrice = referencePrice;
        Quantity = quantity;
        Description = description;
        ThumbnailUrl = thumbnailUrl;
        PreviewAsset = previewAsset;
        Slug = slug;
        UpdatedAt = updatedAt;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
