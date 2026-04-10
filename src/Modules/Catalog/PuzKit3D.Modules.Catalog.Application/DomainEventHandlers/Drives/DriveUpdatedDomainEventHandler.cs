using MediatR;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Drives;

internal sealed class DriveUpdatedDomainEventHandler
    : INotificationHandler<DriveUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public DriveUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        DriveUpdatedDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new DriveUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.DriveId,
            notification.Name,
            notification.Description,
            notification.MinVolume,
            notification.QuantityInStock,
            notification.UpdatedAt,
            notification.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
