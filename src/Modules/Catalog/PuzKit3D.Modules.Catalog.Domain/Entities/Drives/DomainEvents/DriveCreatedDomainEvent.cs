using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Drives.DomainEvents;

public sealed record DriveCreatedDomainEvent(
    Guid DriveId,
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    bool IsActive,
    DateTime CreatedAt) : DomainEvent;
