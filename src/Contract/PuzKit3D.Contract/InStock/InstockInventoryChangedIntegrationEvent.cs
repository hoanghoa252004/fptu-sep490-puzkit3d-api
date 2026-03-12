using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock;

public sealed record InstockInventoryChangedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid InventoryId,
    Guid VariantId,
    int TotalQuantity) : IIntegrationEvent;
