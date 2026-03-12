using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Events.InstockInventories;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;

public sealed class InstockInventory : Entity<InstockInventoryId>
{
    public InstockProductVariantId InstockProductVariantId { get; private set; } = null!;
    public int TotalQuantity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InstockInventory(
        InstockInventoryId id,
        InstockProductVariantId instockProductVariantId,
        int totalQuantity,
        DateTime createdAt) : base(id)
    {
        InstockProductVariantId = instockProductVariantId;
        TotalQuantity = totalQuantity;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private InstockInventory() : base()
    {
    }

    public static ResultT<InstockInventory> Create(
        InstockProductVariantId instockProductVariantId,
        int totalQuantity,
        DateTime? createdAt = null)
    {
        if (totalQuantity < 0)
            return Result.Failure<InstockInventory>(InstockInventoryError.InvalidQuantity());

        var inventoryId = InstockInventoryId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var inventory = new InstockInventory(
            inventoryId,
            instockProductVariantId,
            totalQuantity,
            timestamp);

        // Raise domain event
        inventory.RaiseDomainEvent(new InstockInventoryCreatedDomainEvent(
            inventory.Id.Value,
            inventory.InstockProductVariantId.Value,
            inventory.TotalQuantity));

        return Result.Success(inventory);
    }

    public Result AddStock(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(InstockInventoryError.InvalidQuantity());

        TotalQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockInventoryUpdatedDomainEvent(
            Id.Value,
            InstockProductVariantId.Value,
            TotalQuantity));

        return Result.Success();
    }

    public Result ReduceStock(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(InstockInventoryError.InvalidQuantity());

        if (TotalQuantity < quantity)
            return Result.Failure(InstockInventoryError.InsufficientQuantity(TotalQuantity, quantity));

        TotalQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockInventoryUpdatedDomainEvent(
            Id.Value,
            InstockProductVariantId.Value,
            TotalQuantity));

        return Result.Success();
    }

    public Result SetStock(int quantity)
    {
        if (quantity < 0)
            return Result.Failure(InstockInventoryError.InvalidQuantity());

        TotalQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;

        // Raise domain event
        RaiseDomainEvent(new InstockInventoryUpdatedDomainEvent(
            Id.Value,
            InstockProductVariantId.Value,
            TotalQuantity));

        return Result.Success();
    }
}
