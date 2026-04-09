using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.UpdateCapabilityDrive;

public sealed record UpdateCapabilityDriveCommand(
    Guid Id,
    int? Quantity) : ICommand;
