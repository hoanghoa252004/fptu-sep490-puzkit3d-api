using MediatR;
using PuzKit3D.Contract.Cart;
using PuzKit3D.Modules.Cart.Domain.Events.Carts;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.DomainEventHandlers;

internal sealed class CartItemAddedDomainEventHandler : INotificationHandler<CartItemAddedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CartItemAddedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CartItemAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new CartItemAddedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.CartId,
            Guid.Empty,
            notification.ItemId,
            notification.Quantity,
            notification.UnitPrice);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
