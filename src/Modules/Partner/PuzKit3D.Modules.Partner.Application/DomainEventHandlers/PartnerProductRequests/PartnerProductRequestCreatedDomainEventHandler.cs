using MediatR;
using PuzKit3D.Contract.Partner.PartnerProductRequests;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.PartnerProductRequest;

internal sealed class PartnerProductRequestCreatedDomainEventHandler
    : INotificationHandler<PartnerProductRequestCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartnerProductRequestCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(PartnerProductRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartnerProductRequestCreatedIntegrationEvent
        (
            Guid.NewGuid (),
            notification.OccurredOn,
            notification.PartnerProductRequestId,
            notification.CustomerId,
            notification.PartnerId,
            notification.Items.Select(i => new PartnerProductRequestItemDto(
                i.PartnerProductId
                )).ToList(),
            notification.CreatedAt
        );

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
