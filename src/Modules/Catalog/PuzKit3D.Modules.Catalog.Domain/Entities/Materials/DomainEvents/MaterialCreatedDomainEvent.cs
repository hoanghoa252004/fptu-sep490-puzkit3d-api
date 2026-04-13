using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Materials.DomainEvents;

public sealed record MaterialCreatedDomainEvent(
    Guid MaterialId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    decimal BasePrice,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : DomainEvent;
