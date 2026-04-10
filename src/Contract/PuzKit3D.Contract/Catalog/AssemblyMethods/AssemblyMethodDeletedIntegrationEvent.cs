using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.AssemblyMethods;

public sealed record AssemblyMethodDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid AssemblyMethodId,
    DateTime DeletedAt) : IIntegrationEvent;
