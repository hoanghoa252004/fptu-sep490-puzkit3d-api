using MediatR;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrders;

internal sealed class InstockOrderStatusChangedDomainEventHandler
    : INotificationHandler<InstockOrderStatusChangedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockOrderStatusChangedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockOrderStatusChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockOrderStatusChangedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.OrderId,
            notification.Code,
            notification.CustomerId,
            notification.NewStatus.ToString(),
            notification.ChangedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
