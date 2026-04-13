using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.CreateCapability;

public sealed record CreateCapabilityCommand(
    string Name,
    string Slug,
    decimal FactorPercentage,
    string? Description,
    bool IsActive) : ICommandT<Guid>;
