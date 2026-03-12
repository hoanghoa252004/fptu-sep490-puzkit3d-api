using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockPrices;

internal sealed class InstockPriceDeletedDomainEventHandler
    : INotificationHandler<InstockPriceDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockPriceDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockPriceDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockPriceDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.PriceId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
