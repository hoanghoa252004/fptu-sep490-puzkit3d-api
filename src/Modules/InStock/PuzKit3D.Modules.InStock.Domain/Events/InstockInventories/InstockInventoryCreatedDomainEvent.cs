using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockInventories;

public sealed record InstockInventoryCreatedDomainEvent(
    Guid InventoryId,
    Guid VariantId,
    int TotalQuantity) : DomainEvent;
