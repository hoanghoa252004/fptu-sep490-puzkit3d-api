using MediatR;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Capabilities;

internal sealed class CapabilityCreatedDomainEventHandler
    : INotificationHandler<CapabilityCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CapabilityCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CapabilityCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new CapabilityCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.CapabilityId,
            notification.Name,
            notification.Slug,
            notification.Description,
            notification.IsActive,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
