using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.ValueObjects;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;

public sealed class InstockProductPriceDetail : Entity<InstockProductPriceDetailId>
{
    public InstockPriceId InstockPriceId { get; private set; } = null!;
    public InstockProductVariantId InstockProductVariantId { get; private set; } = null!;
    public Money UnitPrice { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InstockProductPriceDetail(
        InstockProductPriceDetailId id,
        InstockPriceId instockPriceId,
        InstockProductVariantId instockProductVariantId,
        Money unitPrice,
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
        var money = Money.Create(unitPrice);
        var timestamp = createdAt ?? DateTime.UtcNow;
        var priceDetail = new InstockProductPriceDetail(
            priceDetailId,
            instockPriceId,
            instockProductVariantId,
            money,
            isActive,
            timestamp);

        // Raise domain event
        priceDetail.RaiseDomainEvent(new InstockProductPriceDetailCreatedDomainEvent(
            priceDetail.Id.Value,
            priceDetail.InstockPriceId.Value,
            priceDetail.InstockProductVariantId.Value,
            priceDetail.UnitPrice.Amount,
            priceDetail.IsActive));

        return Result.Success(priceDetail);
    }

    public Result UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 10000)
            return Result.Failure(InstockProductPriceDetailError.InvalidUnitPrice());

        UnitPrice = Money.Create(unitPrice);
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductPriceDetailUpdatedDomainEvent(
            Id.Value,
            InstockPriceId.Value,
            InstockProductVariantId.Value,
            UnitPrice.Amount,
            IsActive));

        return Result.Success();
    }

    public Result PartialUpdate(decimal? unitPrice = null)
    {
        if (unitPrice.HasValue)
        {
            if (unitPrice.Value < 10000)
                return Result.Failure(InstockProductPriceDetailError.InvalidUnitPrice());

            UnitPrice = Money.Create(unitPrice.Value);
        }

        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductPriceDetailUpdatedDomainEvent(
            Id.Value,
            InstockPriceId.Value,
            InstockProductVariantId.Value,
            UnitPrice.Amount,
            IsActive));

        return Result.Success();
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductPriceDetailActivatedDomainEvent(
            Id.Value,
            IsActive));
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockProductPriceDetailActivatedDomainEvent(
            Id.Value,
            IsActive));
    }
}
