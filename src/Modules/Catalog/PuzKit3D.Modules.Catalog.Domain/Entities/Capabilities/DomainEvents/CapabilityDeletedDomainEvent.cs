using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities.DomainEvents;

public sealed record CapabilityDeletedDomainEvent(
    Guid CapabilityId,
    DateTime DeletedAt) : DomainEvent;
