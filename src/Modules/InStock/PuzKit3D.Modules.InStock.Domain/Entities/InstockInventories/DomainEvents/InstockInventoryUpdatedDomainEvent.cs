using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories.DomainEvents;

public sealed record InstockInventoryUpdatedDomainEvent(
    Guid InventoryId,
    Guid VariantId,
    int TotalQuantity) : DomainEvent;
