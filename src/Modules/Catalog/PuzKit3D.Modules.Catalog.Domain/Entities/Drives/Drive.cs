using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives.DomainEvents;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

public class Drive : AggregateRoot<DriveId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int? MinVolume { get; private set; }
    public int QuantityInStock { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public ICollection<CapabilityDrive> CapabilityDrives { get; private set; } = new List<CapabilityDrive>();

    private Drive(
        DriveId id,
        string name,
        string? description,
        int? minVolume,
        int quantityInStock,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Description = description;
        MinVolume = minVolume;
        QuantityInStock = quantityInStock;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Drive() : base()
    {
    }

    public static ResultT<Drive> Create(
        string name,
        string? description = null,
        int? minVolume = null,
        int quantityInStock = 0,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        var driveId = DriveId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var drive = new Drive(
            driveId,
            name,
            description,
            minVolume,
            quantityInStock,
            isActive,
            timestamp);

        drive.RaiseDomainEvent(new DriveCreatedDomainEvent(
            drive.Id.Value,
            drive.Name,
            drive.Description,
            drive.MinVolume,
            drive.QuantityInStock,
            drive.IsActive,
            drive.CreatedAt));

        return Result.Success(drive);
    }

    public Result Update(
        string name,
        string? description = null,
        int? minVolume = null,
        int quantityInStock = 0,
        bool isActive = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Drive>(DriveError.InvalidName());
        Name = name;
        Description = description;
        MinVolume = minVolume;
        QuantityInStock = quantityInStock;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new DriveUpdatedDomainEvent(
            Id.Value,
            Name,
            Description,
            MinVolume,
            QuantityInStock,
            UpdatedAt,
            IsActive));

        return Result.Success();
    }

    public Result ReduceQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(DriveError.InvalidQuantity());

        if (QuantityInStock < quantity)
            return Result.Failure(DriveError.InsufficientQuantity(QuantityInStock, quantity));

        QuantityInStock -= quantity;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void Delete()
    {
        RaiseDomainEvent(new DriveDeletedDomainEvent(
            Id.Value,
            DateTime.UtcNow));
    }
}
