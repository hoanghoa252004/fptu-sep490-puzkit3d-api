using MediatR;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrders;

internal sealed class OrderCancelledRefundCoinDomainEventHandler
    : INotificationHandler<OrderCancelledRefundCoinDomainEvent>
{
    private readonly IEventBus _eventBus;

    public OrderCancelledRefundCoinDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        OrderCancelledRefundCoinDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderCancelledRefundCoinIntegrationEvent(
            notification.Id,
            notification.OccurredOn,
            notification.OrderId,
            notification.OrderCode,
            notification.CustomerId,
            notification.GrandTotalAmount,
            notification.CancelledAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
