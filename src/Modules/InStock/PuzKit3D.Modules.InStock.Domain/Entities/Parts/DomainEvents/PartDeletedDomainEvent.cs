using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Parts.DomainEvents;

public sealed record PartDeletedDomainEvent(
    Guid PartId) : DomainEvent;
