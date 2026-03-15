using MediatR;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Capabilities;

internal sealed class CapabilityDeletedDomainEventHandler
    : INotificationHandler<CapabilityDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CapabilityDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CapabilityDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new CapabilityDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.CapabilityId,
            notification.DeletedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
