using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockInventories;

public sealed record InstockInventoryDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid InventoryId,
    Guid VariantId) : IIntegrationEvent;
