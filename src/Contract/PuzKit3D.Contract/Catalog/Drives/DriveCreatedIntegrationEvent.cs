using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Drives;

public sealed record DriveCreatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid DriveId,
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    bool IsActive,
    DateTime CreatedAt) : IntegrationEvent(EventId, OccurredOn);
