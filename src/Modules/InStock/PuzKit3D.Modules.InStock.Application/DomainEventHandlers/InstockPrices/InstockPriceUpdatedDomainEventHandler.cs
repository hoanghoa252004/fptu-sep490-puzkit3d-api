using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockPrices;

internal sealed class InstockPriceUpdatedDomainEventHandler 
    : INotificationHandler<InstockPriceUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockPriceUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockPriceUpdatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockPriceUpdatedIntegrationEvent(
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
