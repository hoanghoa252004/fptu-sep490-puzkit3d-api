using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.AssemblyMethods;

public sealed record AssemblyMethodUpdatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid AssemblyMethodId,
    string Name,
    string Slug,
    string? Description,
    DateTime UpdatedAt) : IntegrationEvent(EventId, OccurredOn);
