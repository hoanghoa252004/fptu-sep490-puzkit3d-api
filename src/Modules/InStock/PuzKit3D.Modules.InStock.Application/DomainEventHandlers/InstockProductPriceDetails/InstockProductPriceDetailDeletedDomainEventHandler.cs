using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProductPriceDetails;

internal sealed class InstockProductPriceDetailDeletedDomainEventHandler
    : INotificationHandler<InstockProductPriceDetailDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductPriceDetailDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockProductPriceDetailDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockProductPriceDetailDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.PriceDetailId,
            notification.PriceId,
            notification.VariantId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
