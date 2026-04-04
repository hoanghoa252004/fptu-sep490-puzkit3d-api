using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

public sealed class AssemblyMethodReplica : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private AssemblyMethodReplica(
        Guid id,
        string name,
        string slug,
        string? description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private AssemblyMethodReplica() : base()
    {
    }

    public static AssemblyMethodReplica Create(
        Guid id,
        string name,
        string slug,
        string? description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new AssemblyMethodReplica(
            id,
            name,
            slug,
            description,
            isActive,
            createdAt,
            updatedAt);
    }

    public void Update(
        string name,
        string slug,
        string? description,
        bool isActive,
        DateTime updatedAt)
    {
        Name = name;
        Slug = slug;
        Description = description;
        IsActive = isActive;
        UpdatedAt = updatedAt;
    }
}
