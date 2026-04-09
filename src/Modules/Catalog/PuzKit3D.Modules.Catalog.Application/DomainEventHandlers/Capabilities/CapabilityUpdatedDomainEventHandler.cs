using MediatR;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Capabilities;

internal sealed class CapabilityUpdatedDomainEventHandler
    : INotificationHandler<CapabilityUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public CapabilityUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(CapabilityUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new CapabilityUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.CapabilityId,
            notification.Name,
            notification.Slug,
            notification.FactorPercentage,
            notification.Description,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
