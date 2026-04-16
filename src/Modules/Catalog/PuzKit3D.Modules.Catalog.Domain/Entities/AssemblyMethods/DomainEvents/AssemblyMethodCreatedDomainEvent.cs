using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods.DomainEvents;

public sealed record AssemblyMethodCreatedDomainEvent(
    Guid AssemblyMethodId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : DomainEvent;
