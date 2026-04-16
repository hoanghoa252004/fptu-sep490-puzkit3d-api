using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities.DomainEvents;

public sealed record CapabilityCreatedDomainEvent(
    Guid CapabilityId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : DomainEvent;
