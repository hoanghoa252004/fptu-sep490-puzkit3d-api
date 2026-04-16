using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Capabilities;

public sealed record CapabilityDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid CapabilityId,
    DateTime DeletedAt) : IIntegrationEvent;
