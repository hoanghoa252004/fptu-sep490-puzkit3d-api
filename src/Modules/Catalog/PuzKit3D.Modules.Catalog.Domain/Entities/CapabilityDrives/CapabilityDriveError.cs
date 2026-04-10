using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;

public static class CapabilityDriveError
{
    public static Error AlreadyExists(CapabilityId capabilityId, DriveId driveId) => Error.Conflict(
            "CapabilityDrive.AlreadyExists",
            $"A capability drive with capability ID '{capabilityId}' and drive ID '{driveId}' already exists.");
}
