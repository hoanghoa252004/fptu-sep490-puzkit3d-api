using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

public sealed class CapabilityReplica : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public decimal FactorPercentage { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private CapabilityReplica(
        Guid id,
        string name,
        string slug,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        FactorPercentage = factorPercentage;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private CapabilityReplica() : base()
    {
    }

    public static CapabilityReplica Create(
        Guid id,
        string name,
        string slug,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new CapabilityReplica(
            id,
            name,
            slug,
            factorPercentage,
            description,
            isActive,
            createdAt,
            updatedAt);
    }

    public void Update(
        string name,
        string slug,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime updatedAt)
    {
        Name = name;
        Slug = slug;
        Description = description;
        FactorPercentage = factorPercentage;
        IsActive = isActive;
        UpdatedAt = updatedAt;
    }
}
