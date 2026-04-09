using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Materials.DomainEvents;

public sealed record MaterialUpdatedDomainEvent(
    Guid MaterialId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    decimal BasePrice,
    string? Description,
    DateTime UpdatedAt) : DomainEvent;
