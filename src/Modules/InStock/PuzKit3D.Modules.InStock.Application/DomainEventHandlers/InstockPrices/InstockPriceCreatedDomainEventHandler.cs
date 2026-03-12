using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Domain.Events.InstockPrices;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockPrices;

internal sealed class InstockPriceCreatedDomainEventHandler 
    : INotificationHandler<InstockPriceCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockPriceCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockPriceCreatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockPriceChangedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.PriceId,
            domainEvent.Name,
            domainEvent.EffectiveFrom,
            domainEvent.EffectiveTo,
            domainEvent.Priority,
            domainEvent.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
