using MediatR;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Drives;

internal sealed class DriveDeletedDomainEventHandler
    : INotificationHandler<DriveDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public DriveDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(DriveDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new DriveDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.DriveId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
