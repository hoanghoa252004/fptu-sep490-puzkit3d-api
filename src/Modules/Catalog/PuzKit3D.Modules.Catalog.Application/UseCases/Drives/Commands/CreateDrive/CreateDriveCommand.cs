using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.CreateDrive;

public sealed record CreateDriveCommand(
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    bool IsActive) : ICommandT<Guid>;
