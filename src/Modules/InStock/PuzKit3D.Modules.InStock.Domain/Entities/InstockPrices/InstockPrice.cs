using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;

public sealed class InstockPrice : AggregateRoot<InstockPriceId>
{
    public string Name { get; private set; } = null!;
    public DateTime EffectiveFrom { get; private set; }
    public DateTime EffectiveTo { get; private set; }
    public int Priority { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InstockPrice(
        InstockPriceId id,
        string name,
        DateTime effectiveFrom,
        DateTime effectiveTo,
        int priority,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Priority = priority;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private InstockPrice() : base()
    {
    }

    public static ResultT<InstockPrice> Create(
        string name,
        DateTime effectiveFrom,
        DateTime effectiveTo,
        int priority,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<InstockPrice>(InstockPriceError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<InstockPrice>(InstockPriceError.NameTooLong(name.Length));

        if (effectiveFrom >= effectiveTo)
            return Result.Failure<InstockPrice>(InstockPriceError.InvalidDateRange());

        if (priority <= 0)
            return Result.Failure<InstockPrice>(InstockPriceError.InvalidPriority());

        var priceId = InstockPriceId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var price = new InstockPrice(
            priceId,
            name,
            effectiveFrom,
            effectiveTo,
            priority,
            isActive,
            timestamp);

        return Result.Success(price);
    }

    public Result Update(
        string name,
        DateTime effectiveFrom,
        DateTime effectiveTo,
        int priority)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(InstockPriceError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(InstockPriceError.NameTooLong(name.Length));

        if (effectiveFrom >= effectiveTo)
            return Result.Failure(InstockPriceError.InvalidDateRange());

        if (priority <= 0)
            return Result.Failure(InstockPriceError.InvalidPriority());

        Name = name;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Priority = priority;
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
