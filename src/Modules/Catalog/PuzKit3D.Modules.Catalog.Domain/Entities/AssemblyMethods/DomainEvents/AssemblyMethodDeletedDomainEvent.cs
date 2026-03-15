using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods.DomainEvents;

public sealed record AssemblyMethodDeletedDomainEvent(
    Guid AssemblyMethodId,
    DateTime DeletedAt) : DomainEvent;
