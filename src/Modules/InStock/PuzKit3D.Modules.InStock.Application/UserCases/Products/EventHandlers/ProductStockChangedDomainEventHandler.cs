using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Domain.Events.Products;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.EventHandlers;

internal sealed class ProductStockChangedDomainEventHandler : INotificationHandler<ProductStockChangedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public ProductStockChangedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(ProductStockChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Publish integration event to other modules
        var integrationEvent = new ProductStockChangedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.ProductId,
            notification.OldStock,
            notification.NewStock,
            notification.Quantity);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
