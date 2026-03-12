using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProductPriceDetails;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProductPriceDetails;

internal sealed class InstockProductPriceDetailUpdatedDomainEventHandler 
    : INotificationHandler<InstockProductPriceDetailUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductPriceDetailUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockProductPriceDetailUpdatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockProductPriceDetailUpdatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.PriceDetailId,
            domainEvent.PriceId,
            domainEvent.VariantId,
            domainEvent.UnitPrice,
            domainEvent.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
