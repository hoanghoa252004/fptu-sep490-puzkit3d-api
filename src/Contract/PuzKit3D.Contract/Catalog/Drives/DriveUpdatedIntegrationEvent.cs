using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Drives;

public sealed record DriveUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid DriveId,
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    DateTime UpdatedAt,
    bool IsActive) : IIntegrationEvent;
