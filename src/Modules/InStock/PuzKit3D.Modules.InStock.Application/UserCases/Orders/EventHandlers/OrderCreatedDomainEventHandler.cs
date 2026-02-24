using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Domain.Events.Orders;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Orders.EventHandlers;

internal sealed class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public OrderCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Publish integration event to other modules
        var integrationEvent = new OrderCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.OrderId,
            notification.UserId,
            notification.TotalMoney,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
