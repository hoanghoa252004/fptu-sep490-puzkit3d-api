using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.UpdateDrive;

public sealed record UpdateDriveCommand(
    Guid Id,
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    bool IsActive) : ICommand;
