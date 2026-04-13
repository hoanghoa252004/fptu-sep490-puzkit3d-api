using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.CreateCapabilityDrive;

public sealed record CreateCapabilityDriveCommand(
    Guid CapabilityId,
    Guid DriveId) : ICommandT<object>;
