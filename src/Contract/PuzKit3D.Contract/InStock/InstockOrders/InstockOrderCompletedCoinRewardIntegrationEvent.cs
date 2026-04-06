using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record InstockOrderCompletedCoinRewardIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    Guid CustomerId,
    string PaymentMethod,
    decimal GrandTotalAmount,
    int UsedCoinAmount) : IIntegrationEvent;
