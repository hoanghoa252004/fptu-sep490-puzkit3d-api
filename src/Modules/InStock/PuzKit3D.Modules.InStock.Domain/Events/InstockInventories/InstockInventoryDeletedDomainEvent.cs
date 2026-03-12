using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockInventories;

public sealed record InstockInventoryDeletedDomainEvent(
    Guid InventoryId,
    Guid VariantId) : DomainEvent;
