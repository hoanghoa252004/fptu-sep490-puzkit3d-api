using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Materials;

public sealed record MaterialDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid MaterialId,
    DateTime DeletedAt) : IIntegrationEvent;
