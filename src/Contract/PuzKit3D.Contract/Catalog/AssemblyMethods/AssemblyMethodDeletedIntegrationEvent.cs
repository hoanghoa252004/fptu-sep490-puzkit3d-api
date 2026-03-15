using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.AssemblyMethods;

public sealed record AssemblyMethodDeletedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid AssemblyMethodId,
    DateTime DeletedAt) : IntegrationEvent(EventId, OccurredOn);
