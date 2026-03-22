using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Capabilities;

public sealed record CapabilityUpdatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid CapabilityId,
    string Name,
    string Slug,
    string? Description,
    DateTime UpdatedAt) : IntegrationEvent(EventId, OccurredOn);
