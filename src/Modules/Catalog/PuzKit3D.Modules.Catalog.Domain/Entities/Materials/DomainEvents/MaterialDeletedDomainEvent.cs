using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Materials.DomainEvents;

public sealed record MaterialDeletedDomainEvent(
    Guid MaterialId,
    DateTime DeletedAt) : DomainEvent;
