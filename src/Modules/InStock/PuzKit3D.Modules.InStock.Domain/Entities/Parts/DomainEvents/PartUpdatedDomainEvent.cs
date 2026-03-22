using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Parts.DomainEvents;

public sealed record PartUpdatedDomainEvent(
    Guid PartId,
    string Name,
    string PartType,
    string Code,
    int Quantity,
    Guid InstockProductId) : DomainEvent;
