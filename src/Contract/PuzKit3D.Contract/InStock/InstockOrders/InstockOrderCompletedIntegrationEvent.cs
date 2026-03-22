using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record InstockOrderCompletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string Code,
    Guid CustomerId,
    List<OrderDetailCompletedInfo> OrderDetails,
    DateTime CompletedAt) : IIntegrationEvent;

public sealed record OrderDetailCompletedInfo(
    Guid OrderDetailId,
    Guid VariantId,
    Guid ProductId,
    int Quantity);
