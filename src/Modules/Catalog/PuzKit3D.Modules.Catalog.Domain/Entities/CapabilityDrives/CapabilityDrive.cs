using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Domain.Results;

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

    public static ResultT<CapabilityDrive> Create(
        CapabilityId capabilityId, 
        DriveId driveId)
    {
        var capabilityDrive = new CapabilityDrive(
            capabilityId, 
            driveId);
        return Result.Success(capabilityDrive);
    }

}
