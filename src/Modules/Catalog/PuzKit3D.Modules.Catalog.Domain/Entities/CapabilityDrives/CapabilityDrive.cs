using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;

public class CapabilityDrive
{
    public CapabilityId CapabilityId { get; private set; } = null!;
    public DriveId DriveId { get; private set; } = null!;

    private CapabilityDrive(CapabilityId capabilityId, DriveId driveId)
    {
        CapabilityId = capabilityId;
        DriveId = driveId;
    }

    private CapabilityDrive()
    {
    }

    public static CapabilityDrive Create(CapabilityId capabilityId, DriveId driveId)
    {
        return new CapabilityDrive(capabilityId, driveId);
    }

}
