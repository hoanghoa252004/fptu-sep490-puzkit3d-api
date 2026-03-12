using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record InstockOrderCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    Guid CustomerId,
    List<Guid> CartItemIds) : IIntegrationEvent;
