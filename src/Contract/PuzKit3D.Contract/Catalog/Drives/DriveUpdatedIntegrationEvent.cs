using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Drives;

public sealed record DriveUpdatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid DriveId,
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    DateTime UpdatedAt) : IntegrationEvent(EventId, OccurredOn);
