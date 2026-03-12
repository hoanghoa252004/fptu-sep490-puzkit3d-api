using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

public sealed class TopicReplica : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public Guid? ParentId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private TopicReplica(
        Guid id,
        string name,
        string slug,
        string? description,
        Guid? parentId,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ParentId = parentId;
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
        string? description,
        Guid? parentId,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new TopicReplica(
            id,
            name,
            slug,
            description,
            parentId,
            isActive,
            createdAt,
            updatedAt);
    }

    public void Update(
        string name,
        string slug,
        string? description,
        Guid? parentId,
        bool isActive,
        DateTime updatedAt)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ParentId = parentId;
        IsActive = isActive;
        UpdatedAt = updatedAt;
    }
}
