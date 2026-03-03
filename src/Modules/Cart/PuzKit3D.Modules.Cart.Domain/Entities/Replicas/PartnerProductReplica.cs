using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class PartnerProductReplica : Entity<Guid>
{
    public Guid PartnerId { get; private set; }
    public string PartnerProductSku { get; private set; }
    public string Name { get; private set; }
    public string? BriefDescription { get; private set; }
    public string? DetailDescription { get; private set; }
    public string? ProductCatalog { get; private set; }
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
        string partnerProductSku,
        string name,
        string? briefDescription,
        string? detailDescription,
        string? productCatalog,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new PartnerProductReplica
        {
            Id = id,
            PartnerId = partnerId,
            PartnerProductSku = partnerProductSku,
            Name = name,
            BriefDescription = briefDescription,
            DetailDescription = detailDescription,
            ProductCatalog = productCatalog,
            ThumbnailUrl = thumbnailUrl,
            PreviewAsset = previewAsset,
            Slug = slug,
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
}
