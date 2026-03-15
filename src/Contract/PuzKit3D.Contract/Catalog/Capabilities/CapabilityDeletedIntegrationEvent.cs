using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Capabilities;

public sealed record CapabilityDeletedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid CapabilityId,
    DateTime DeletedAt) : IntegrationEvent(EventId, OccurredOn);
