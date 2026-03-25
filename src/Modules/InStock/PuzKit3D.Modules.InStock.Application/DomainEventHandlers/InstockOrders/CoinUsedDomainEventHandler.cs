using MediatR;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrders;

internal sealed class CoinUsedDomainEventHandler
    : INotificationHandler<CoinUsedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CoinUsedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        CoinUsedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new CoinUsedIntegrationEvent(
            notification.Id,
            notification.OccurredOn,
            notification.OrderId,
            notification.OrderCode,
            notification.CustomerId,
            notification.UsedCoinAmount,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
