using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record CoinUsedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    decimal UsedCoinAmount,
    DateTime CreatedAt) : IIntegrationEvent;
