using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

public sealed class DriveReplica : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int? MinVolume { get; private set; }
    public int QuantityInStock { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private DriveReplica(
        Guid id,
        string name,
        string? description,
        int? minVolume,
        int quantityInStock,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Name = name;
        Description = description;
        MinVolume = minVolume;
        QuantityInStock = quantityInStock;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private DriveReplica() : base()
    {
    }

    public static DriveReplica Create(
        Guid id,
        string name,
        string? description,
        int? minVolume,
        int quantityInStock,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new DriveReplica(
            id,
            name,
            description,
            minVolume,
            quantityInStock,
            isActive,
            createdAt,
            updatedAt);
    }

    public void Update(
        string name,
        string? description,
        int? minVolume,
        int quantityInStock,
        bool isActive,
        DateTime updatedAt)
    {
        Name = name;
        Description = description;
        MinVolume = minVolume;
        QuantityInStock = quantityInStock;
        IsActive = isActive;
        UpdatedAt = updatedAt;
    }
}
