using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Drives;

public sealed record DriveDeletedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid DriveId) : IntegrationEvent(EventId, OccurredOn);
