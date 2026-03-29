using MediatR;
using PuzKit3D.Contract.Partner.Partners;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.Partners;

internal sealed class PartnerDeletedDomainEventHandler
    : INotificationHandler<PartnerDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartnerDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        PartnerDeletedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new PartnerDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.PartnerId,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
