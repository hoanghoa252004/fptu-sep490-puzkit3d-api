using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProducts;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProducts;

internal sealed class InstockProductDeletedDomainEventHandler
    : INotificationHandler<InstockProductDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockProductDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockProductDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.ProductId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
