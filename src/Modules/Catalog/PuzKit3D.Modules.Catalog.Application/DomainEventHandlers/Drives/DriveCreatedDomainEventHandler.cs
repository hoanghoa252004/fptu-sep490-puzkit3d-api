using MediatR;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Drives;

internal sealed class DriveCreatedDomainEventHandler
    : INotificationHandler<DriveCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public DriveCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(DriveCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new DriveCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.DriveId,
            notification.Name,
            notification.Description,
            notification.MinVolume,
            notification.QuantityInStock,
            notification.IsActive,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
