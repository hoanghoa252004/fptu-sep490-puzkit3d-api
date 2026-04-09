using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;

public class CapabilityDrive : AggregateRoot<CapabilityDriveId>
{
    public CapabilityId CapabilityId { get; private set; } = null!;
    public DriveId DriveId { get; private set; } = null!;
    public int Quantity { get; private set; }

    private CapabilityDrive(
        CapabilityDriveId id,
        CapabilityId capabilityId,
        DriveId driveId,
        int quantity) : base(id)
    {
        CapabilityId = capabilityId;
        DriveId = driveId;
        Quantity = quantity;
    }

    private CapabilityDrive() : base()
    {
    }

    public static CapabilityDrive Create(
        CapabilityId capabilityId,
        DriveId driveId,
        int quantity)
    {
        var id = CapabilityDriveId.Create();
        return new CapabilityDrive(id, capabilityId, driveId, quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}
