using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;

public class PartnerProduct : AggregateRoot<PartnerProductId>
{
    public PartnerId PartnerId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public decimal ReferencePrice { get; private set; }
    public string? Description { get; private set; }
    public string ThumbnailUrl { get; private set; } = null!;
    public string PreviewAsset { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private PartnerProduct(
        PartnerProductId id,
        PartnerId partnerId,
        string name,
        decimal referencePrice,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        string? description,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        PartnerId = partnerId;
        Name = name;
        ReferencePrice = referencePrice;
        ThumbnailUrl = thumbnailUrl;
        PreviewAsset = previewAsset;
        Slug = slug;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private PartnerProduct() : base()
    {
    }

    public static ResultT<PartnerProduct> Create(
        PartnerId partnerId,
        string name,
        decimal referencePrice,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<PartnerProduct>(PartnerProductError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<PartnerProduct>(PartnerProductError.NameTooLong(name.Length));

        if (referencePrice < 0)
            return Result.Failure<PartnerProduct>(PartnerProductError.InvalidReferencePrice());

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<PartnerProduct>(PartnerProductError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure<PartnerProduct>(PartnerProductError.SlugTooLong(slug.Length));

        var productId = PartnerProductId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var product = new PartnerProduct(
            productId,
            partnerId,
            name,
            referencePrice,
            thumbnailUrl,
            previewAsset,
            slug,
            description,
            isActive,
            timestamp);

        return Result.Success(product);
    }

    public Result Update(
        string name,
        decimal referencePrice,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PartnerProductError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(PartnerProductError.NameTooLong(name.Length));

        if (referencePrice < 0)
            return Result.Failure(PartnerProductError.InvalidReferencePrice());

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(PartnerProductError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure(PartnerProductError.SlugTooLong(slug.Length));

        Name = name;
        ReferencePrice = referencePrice;
        ThumbnailUrl = thumbnailUrl;
        PreviewAsset = previewAsset;
        Slug = slug;
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
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
