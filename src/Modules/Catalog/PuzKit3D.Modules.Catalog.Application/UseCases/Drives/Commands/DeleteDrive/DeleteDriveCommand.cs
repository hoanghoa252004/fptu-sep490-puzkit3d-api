using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.DeleteDrive;

public sealed record DeleteDriveCommand(Guid Id) : ICommand;
