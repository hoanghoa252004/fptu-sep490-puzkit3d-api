using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Drives.DomainEvents;

public sealed record DriveUpdatedDomainEvent(
    Guid DriveId,
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    DateTime UpdatedAt,
    bool IsActive) : DomainEvent;
