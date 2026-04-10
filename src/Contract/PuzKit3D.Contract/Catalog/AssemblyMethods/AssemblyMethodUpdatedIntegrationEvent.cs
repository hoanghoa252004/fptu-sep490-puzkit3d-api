using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.AssemblyMethods;

public sealed record AssemblyMethodUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid AssemblyMethodId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    string? Description,
    DateTime UpdatedAt,
    bool IsActive) : IIntegrationEvent;
