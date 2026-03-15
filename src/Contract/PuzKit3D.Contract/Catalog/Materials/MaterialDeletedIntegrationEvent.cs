using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Materials;

public sealed record MaterialDeletedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid MaterialId,
    DateTime DeletedAt) : IntegrationEvent(EventId, OccurredOn);
