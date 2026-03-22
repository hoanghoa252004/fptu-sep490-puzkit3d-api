using MediatR;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.PartnerProducts;

internal sealed class PartnerProductActivatedDomainEventHandler
    : INotificationHandler<PartnerProductActivatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartnerProductActivatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(PartnerProductActivatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartnerProductActivatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.ProductId,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}