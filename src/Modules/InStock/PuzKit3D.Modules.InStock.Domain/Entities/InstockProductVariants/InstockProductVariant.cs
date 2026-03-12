using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants.DomainEvents;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;

public sealed class InstockProductVariant : Entity<InstockProductVariantId>
{
    public InstockProductId InstockProductId { get; private set; } = null!;
    public string Sku { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public int AssembledLengthMm { get; private set; }
    public int AssembledWidthMm { get; private set; }
    public int AssembledHeightMm { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InstockProductVariant(
        InstockProductVariantId id,
        InstockProductId instockProductId,
        string sku,
        string color,
        int assembledLengthMm,
        int assembledWidthMm,
        int assembledHeightMm,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        InstockProductId = instockProductId;
        Sku = sku;
        Color = color;
        AssembledLengthMm = assembledLengthMm;
        AssembledWidthMm = assembledWidthMm;
        AssembledHeightMm = assembledHeightMm;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private InstockProductVariant() : base()
    {
    }

    public static ResultT<InstockProductVariant> Create(
        InstockProductId instockProductId,
        string sku,
        string color,
        int assembledLengthMm,
        int assembledWidthMm,
        int assembledHeightMm,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return Result.Failure<InstockProductVariant>(InstockProductVariantError.InvalidSku());

        if (sku.Length > 10)
            return Result.Failure<InstockProductVariant>(InstockProductVariantError.SkuTooLong(sku.Length));

        if (string.IsNullOrWhiteSpace(color))
            return Result.Failure<InstockProductVariant>(InstockProductVariantError.InvalidColor());

        if (color.Length > 15)
            return Result.Failure<InstockProductVariant>(InstockProductVariantError.ColorTooLong(color.Length));

        if (assembledLengthMm <= 0 || assembledWidthMm <= 0 || assembledHeightMm <= 0)
            return Result.Failure<InstockProductVariant>(InstockProductVariantError.InvalidDimension());

        var variantId = InstockProductVariantId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var variant = new InstockProductVariant(
            variantId,
            instockProductId,
            sku,
            color,
            assembledLengthMm,
            assembledWidthMm,
            assembledHeightMm,
            isActive,
            timestamp);

        // Raise domain event
        variant.RaiseDomainEvent(new InstockProductVariantCreatedDomainEvent(
            variant.Id.Value,
            variant.InstockProductId.Value,
            variant.Sku,
            variant.Color,
            variant.AssembledLengthMm,
            variant.AssembledWidthMm,
            variant.AssembledHeightMm,
            variant.IsActive));

        Console.WriteLine("Domain Event COUNT: " +variant.DomainEvents.Count());

        return Result.Success(variant);
    }

    public Result Update(
        string sku,
        string color,
        int assembledLengthMm,
        int assembledWidthMm,
        int assembledHeightMm)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return Result.Failure(InstockProductVariantError.InvalidSku());

        if (sku.Length > 10)
            return Result.Failure(InstockProductVariantError.SkuTooLong(sku.Length));

        if (string.IsNullOrWhiteSpace(color))
            return Result.Failure(InstockProductVariantError.InvalidColor());

        if (color.Length > 15)
            return Result.Failure(InstockProductVariantError.ColorTooLong(color.Length));

        if (assembledLengthMm <= 0 || assembledWidthMm <= 0 || assembledHeightMm <= 0)
            return Result.Failure(InstockProductVariantError.InvalidDimension());

        Sku = sku;
        Color = color;
        AssembledLengthMm = assembledLengthMm;
        AssembledWidthMm = assembledWidthMm;
        AssembledHeightMm = assembledHeightMm;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductVariantUpdatedDomainEvent(
            Id.Value,
            InstockProductId.Value,
            Sku,
            Color,
            AssembledLengthMm,
            AssembledWidthMm,
            AssembledHeightMm,
            IsActive));

        return Result.Success();
    }

    public Result PartialUpdate(
        string? sku = null,
        string? color = null,
        int? assembledLengthMm = null,
        int? assembledWidthMm = null,
        int? assembledHeightMm = null,
        bool? isActive = null)
    {
        if (sku is not null)
        {
            if (string.IsNullOrWhiteSpace(sku))
                return Result.Failure(InstockProductVariantError.InvalidSku());

            if (sku.Length > 10)
                return Result.Failure(InstockProductVariantError.SkuTooLong(sku.Length));

            Sku = sku;
        }

        if (color is not null)
        {
            if (string.IsNullOrWhiteSpace(color))
                return Result.Failure(InstockProductVariantError.InvalidColor());

            if (color.Length > 15)
                return Result.Failure(InstockProductVariantError.ColorTooLong(color.Length));

            Color = color;
        }

        if (assembledLengthMm.HasValue)
        {
            if (assembledLengthMm.Value <= 0)
                return Result.Failure(InstockProductVariantError.InvalidDimension());

            AssembledLengthMm = assembledLengthMm.Value;
        }

        if (assembledWidthMm.HasValue)
        {
            if (assembledWidthMm.Value <= 0)
                return Result.Failure(InstockProductVariantError.InvalidDimension());

            AssembledWidthMm = assembledWidthMm.Value;
        }

        if (assembledHeightMm.HasValue)
        {
            if (assembledHeightMm.Value <= 0)
                return Result.Failure(InstockProductVariantError.InvalidDimension());

            AssembledHeightMm = assembledHeightMm.Value;
        }

        if (isActive.HasValue)
        {
            if (isActive.Value == IsActive)
                return Result.Failure(InstockProductVariantError.IsActiveUnchanged(IsActive));

            IsActive = isActive.Value;
        }

        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductVariantUpdatedDomainEvent(
            Id.Value,
            InstockProductId.Value,
            Sku,
            Color,
            AssembledLengthMm,
            AssembledWidthMm,
            AssembledHeightMm,
            IsActive));

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

        RaiseDomainEvent(new InstockProductVariantDeletedDomainEvent(
            Id.Value,
            InstockProductId.Value));
    }
}
