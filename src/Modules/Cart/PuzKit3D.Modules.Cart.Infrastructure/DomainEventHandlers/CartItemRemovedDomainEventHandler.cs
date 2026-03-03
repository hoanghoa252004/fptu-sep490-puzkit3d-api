using MediatR;
using PuzKit3D.Contract.Cart;
using PuzKit3D.Modules.Cart.Domain.Events.Carts;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.DomainEventHandlers;

internal sealed class CartItemRemovedDomainEventHandler : INotificationHandler<CartItemRemovedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CartItemRemovedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CartItemRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new CartItemRemovedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.CartId,
            Guid.Empty,
            notification.ItemId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
