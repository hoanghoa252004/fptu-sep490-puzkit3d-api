using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.AssemblyMethods;

public sealed record AssemblyMethodCreatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid AssemblyMethodId,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : IntegrationEvent(EventId, OccurredOn);
