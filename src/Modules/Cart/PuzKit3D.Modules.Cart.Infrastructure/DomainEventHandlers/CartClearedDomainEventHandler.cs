using MediatR;
using PuzKit3D.Contract.Cart;
using PuzKit3D.Modules.Cart.Domain.Events.Carts;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.DomainEventHandlers;

internal sealed class CartClearedDomainEventHandler : INotificationHandler<CartClearedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CartClearedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CartClearedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new CartClearedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.CartId,
            notification.UserId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
