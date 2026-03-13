using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record InstockOrderPaidSuccessIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string Code,
    Guid CustomerId,
    decimal GrandTotalAmount,
    DateTime PaidAt) : IIntegrationEvent;
