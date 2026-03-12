using MediatR;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrders;

internal sealed class InstockOrderCreatedDomainEventHandler
    : INotificationHandler<InstockOrderCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockOrderCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockOrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockOrderCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.OrderId,
            notification.CustomerId,
            notification.CartItemIds);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
