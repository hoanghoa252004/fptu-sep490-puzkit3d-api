using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

public sealed class TopicReplica : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public Guid? ParentId { get; private set; }
    public decimal FactorPercentage { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private TopicReplica(
        Guid id,
        string name,
        string slug,
        Guid? parentId,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ParentId = parentId;
        FactorPercentage = factorPercentage;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private TopicReplica() : base()
    {
    }

    public static TopicReplica Create(
        Guid id,
        string name,
        string slug,
        Guid? parentId,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new TopicReplica(
            id,
            name,
            slug,
            parentId,
            factorPercentage,
            description,
            isActive,
            createdAt,
            updatedAt);
    }

    public void Update(
        string name,
        string slug,
        Guid? parentId,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime updatedAt)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ParentId = parentId;
        FactorPercentage = factorPercentage;
        IsActive = isActive;
        UpdatedAt = updatedAt;
    }
}
