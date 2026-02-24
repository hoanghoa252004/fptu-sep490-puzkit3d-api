using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Domain.Events.Products;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.EventHandlers;

internal sealed class ProductCreatedDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public ProductCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Publish integration event to other modules
        var integrationEvent = new ProductCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.ProductId,
            notification.Name,
            notification.Price,
            notification.InitialStock);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
