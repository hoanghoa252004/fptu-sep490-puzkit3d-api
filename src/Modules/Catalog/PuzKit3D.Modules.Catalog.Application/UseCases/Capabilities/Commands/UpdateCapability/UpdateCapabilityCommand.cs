using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.UpdateCapability;

public sealed record UpdateCapabilityCommand(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive) : ICommand;
