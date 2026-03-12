using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockInventories;

internal sealed class InstockInventoryDeletedDomainEventHandler
    : INotificationHandler<InstockInventoryDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockInventoryDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockInventoryDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockInventoryDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.InventoryId,
            notification.VariantId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
