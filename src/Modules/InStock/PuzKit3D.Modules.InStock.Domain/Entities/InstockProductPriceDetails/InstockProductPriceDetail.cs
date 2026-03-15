using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails.DomainEvents;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;

public sealed class InstockProductPriceDetail : Entity<InstockProductPriceDetailId>
{
    public InstockPriceId InstockPriceId { get; private set; } = null!;
    public InstockProductVariantId InstockProductVariantId { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InstockProductPriceDetail(
        InstockProductPriceDetailId id,
        InstockPriceId instockPriceId,
        InstockProductVariantId instockProductVariantId,
        decimal unitPrice,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        InstockPriceId = instockPriceId;
        InstockProductVariantId = instockProductVariantId;
        UnitPrice = unitPrice;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private InstockProductPriceDetail() : base()
    {
    }

    public static ResultT<InstockProductPriceDetail> Create(
        InstockPriceId instockPriceId,
        InstockProductVariantId instockProductVariantId,
        decimal unitPrice,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (unitPrice < 10000)
            return Result.Failure<InstockProductPriceDetail>(InstockProductPriceDetailError.InvalidUnitPrice());

        var priceDetailId = InstockProductPriceDetailId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var priceDetail = new InstockProductPriceDetail(
            priceDetailId,
            instockPriceId,
            instockProductVariantId,
            unitPrice,
            isActive,
            timestamp);

        // Raise domain event
        priceDetail.RaiseDomainEvent(new InstockProductPriceDetailCreatedDomainEvent(
            priceDetail.Id.Value,
            priceDetail.InstockPriceId.Value,
            priceDetail.InstockProductVariantId.Value,
            priceDetail.UnitPrice,
            priceDetail.IsActive));

        return Result.Success(priceDetail);
    }

    public Result UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 10000)
            return Result.Failure(InstockProductPriceDetailError.InvalidUnitPrice());

        UnitPrice = unitPrice;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductPriceDetailUpdatedDomainEvent(
            Id.Value,
            InstockPriceId.Value,
            InstockProductVariantId.Value,
            UnitPrice,
            IsActive));

        return Result.Success();
    }

    public Result PartialUpdate(decimal? unitPrice = null, bool? isActive = null)
    {
        if (unitPrice.HasValue)
        {
            if (unitPrice.Value < 10000)
                return Result.Failure(InstockProductPriceDetailError.InvalidUnitPrice());

            UnitPrice = unitPrice.Value;
        }

        if (isActive.HasValue)
        {
            if (isActive.Value == IsActive)
                return Result.Failure(InstockProductPriceDetailError.IsActiveUnchanged(IsActive));

            IsActive = isActive.Value;
        }

        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductPriceDetailUpdatedDomainEvent(
            Id.Value,
            InstockPriceId.Value,
            InstockProductVariantId.Value,
            UnitPrice,
            IsActive));

        return Result.Success();
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        RaiseDomainEvent(new InstockProductPriceDetailDeletedDomainEvent(
            Id.Value,
            InstockPriceId.Value,
            InstockProductVariantId.Value));
    }
}

