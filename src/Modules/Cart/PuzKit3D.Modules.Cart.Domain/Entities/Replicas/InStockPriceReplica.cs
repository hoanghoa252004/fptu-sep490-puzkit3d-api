using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockPriceReplica : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public DateTime EffectiveFrom { get; private set; }
    public DateTime EffectiveTo { get; private set; }
    public int Priority { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InStockPriceReplica() : base()
    {
    }

    public static InStockPriceReplica Create(
        Guid id,
        string name,
        DateTime effectiveFrom,
        DateTime effectiveTo,
        int priority,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new InStockPriceReplica
        {
            Id = id,
            Name = name,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            Priority = priority,
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public bool IsEffectiveAt(DateTime dateTime)
    {
        return IsActive 
            && dateTime >= EffectiveFrom 
            && dateTime <= EffectiveTo;
    }
}
