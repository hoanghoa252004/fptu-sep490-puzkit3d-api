using MediatR;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Materials;

internal sealed class MaterialUpdatedDomainEventHandler
    : INotificationHandler<MaterialUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public MaterialUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(MaterialUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new MaterialUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.MaterialId,
            notification.Name,
            notification.Slug,
            notification.FactorPercentage,
            notification.BasePrice,
            notification.Description,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
