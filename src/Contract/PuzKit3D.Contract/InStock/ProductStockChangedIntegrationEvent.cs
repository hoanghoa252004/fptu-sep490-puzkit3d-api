using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock;

public sealed record ProductStockChangedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid ProductId,
    int OldStock,
    int NewStock,
    int Quantity) : IIntegrationEvent;
