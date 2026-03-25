using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts.DomainEvents;
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
    public int Quantity { get; private set; }
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
        int quantity,
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
        Quantity = quantity;
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
        int quantity,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        // Name
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<PartnerProduct>(PartnerProductError.EmptyName());

        if (name.Length > 30)
            return Result.Failure<PartnerProduct>(PartnerProductError.NameTooLong(name.Length));

        // Reference Price
        if (referencePrice < 50000)
            return Result.Failure<PartnerProduct>(PartnerProductError.InvalidReferencePrice());

        // Quantity
        if (quantity < 0)
            return Result.Failure<PartnerProduct>(PartnerProductError.InvalidQuantity());

        // Thumbnail URL
        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            return Result.Failure<PartnerProduct>(PartnerProductError.EmptyThumbnailUrl());

        // Preview Asset
        if (string.IsNullOrWhiteSpace(previewAsset))
            return Result.Failure<PartnerProduct>(PartnerProductError.EmptyPreviewAsset());
        
        // Slug
        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<PartnerProduct>(PartnerProductError.EmptySlug());

        if (slug.Length > 30)
            return Result.Failure<PartnerProduct>(PartnerProductError.SlugTooLong(slug.Length));

        if (slug.Trim().Contains(" "))
            return Result.Failure<PartnerProduct>(PartnerProductError.InvalidSlug());

        var productId = PartnerProductId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var product = new PartnerProduct(
            productId,
            partnerId,
            name,
            referencePrice,
            quantity,
            thumbnailUrl,
            previewAsset,
            slug,
            description,
            isActive,
            timestamp);

        product.RaiseDomainEvent(new PartnerProductCreatedDomainEvent(
            product.Id.Value,
            product.PartnerId.Value,
            product.Name,
            product.ReferencePrice,
            product.Quantity,
            product.ThumbnailUrl,
            product.PreviewAsset,
            product.Slug,
            product.Description,
            product.IsActive,
            product.CreatedAt));

        return Result.Success(product);
    }

    public Result Update(
        string name,
        decimal referencePrice,
        int quantity,
        string thumbnailUrl,
        string previewAsset,
        string slug,
        string? description = null)
    {
        // Name
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PartnerProductError.EmptyName());

        if (name.Length > 30)
            return Result.Failure(PartnerProductError.NameTooLong(name.Length));

        // Reference Price
        if (referencePrice < 0)
            return Result.Failure(PartnerProductError.InvalidReferencePrice());

        // Quantity
        if (quantity < 0)
            return Result.Failure(PartnerProductError.InvalidQuantity());

        // Thumbnail URL
        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            return Result.Failure(PartnerProductError.EmptyThumbnailUrl());

        // Preview Asset
        if (string.IsNullOrWhiteSpace(previewAsset))
            return Result.Failure(PartnerProductError.EmptyPreviewAsset());

        // Slug
        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(PartnerProductError.EmptySlug());

        if (slug.Length > 30)
            return Result.Failure(PartnerProductError.SlugTooLong(slug.Length));

        if (slug.Trim().Contains(" "))
            return Result.Failure(PartnerProductError.InvalidSlug());

        Name = name;
        ReferencePrice = referencePrice;
        Quantity = quantity;
        ThumbnailUrl = thumbnailUrl;
        PreviewAsset = previewAsset;
        Slug = slug;
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PartnerProductUpdatedDomainEvent(
            Id.Value,
            PartnerId.Value,
            Name,
            ReferencePrice,
            Quantity,
            ThumbnailUrl,
            PreviewAsset,
            Slug,
            Description,
            UpdatedAt));

        return Result.Success();
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PartnerProductActivatedDomainEvent(Id.Value, UpdatedAt));
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PartnerProductDeletedDomainEvent(Id.Value, UpdatedAt));
    }
}
