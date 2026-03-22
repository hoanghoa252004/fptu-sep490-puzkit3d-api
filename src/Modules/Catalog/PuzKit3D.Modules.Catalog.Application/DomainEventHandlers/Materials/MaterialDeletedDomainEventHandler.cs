using MediatR;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Materials;

internal sealed class MaterialDeletedDomainEventHandler
    : INotificationHandler<MaterialDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public MaterialDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(MaterialDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new MaterialDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.MaterialId,
            notification.DeletedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
