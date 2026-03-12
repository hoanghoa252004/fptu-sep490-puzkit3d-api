using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockInventories;

public sealed record InstockInventoryCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid InventoryId,
    Guid VariantId,
    int TotalQuantity) : IIntegrationEvent;
