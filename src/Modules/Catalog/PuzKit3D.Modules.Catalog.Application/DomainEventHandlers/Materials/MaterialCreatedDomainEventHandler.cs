using MediatR;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Materials;

internal sealed class MaterialCreatedDomainEventHandler
    : INotificationHandler<MaterialCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public MaterialCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(MaterialCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new MaterialCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.MaterialId,
            notification.Name,
            notification.Slug,
            notification.Description,
            notification.IsActive,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
