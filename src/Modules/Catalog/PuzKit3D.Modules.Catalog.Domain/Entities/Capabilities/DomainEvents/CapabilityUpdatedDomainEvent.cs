using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities.DomainEvents;

public sealed record CapabilityUpdatedDomainEvent(
    Guid CapabilityId,
    string Name,
    string Slug,
    string? Description,
    DateTime UpdatedAt) : DomainEvent;
