using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods.DomainEvents;

public sealed record AssemblyMethodUpdatedDomainEvent(
    Guid AssemblyMethodId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    string? Description,
    DateTime UpdatedAt) : DomainEvent;
