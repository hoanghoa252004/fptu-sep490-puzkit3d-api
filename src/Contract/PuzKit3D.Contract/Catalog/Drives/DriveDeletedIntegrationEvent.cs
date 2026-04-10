using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Drives;

public sealed record DriveDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid DriveId) : IIntegrationEvent;
