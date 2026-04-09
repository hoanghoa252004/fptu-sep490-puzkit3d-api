using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.DeleteCapabilityDrive;

public sealed record DeleteCapabilityDriveCommand(Guid Id) : ICommand;
