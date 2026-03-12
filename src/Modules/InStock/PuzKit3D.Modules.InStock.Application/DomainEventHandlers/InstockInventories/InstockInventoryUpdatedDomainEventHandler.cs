using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockInventories;

internal sealed class InstockInventoryUpdatedDomainEventHandler 
    : INotificationHandler<InstockInventoryUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockInventoryUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockInventoryUpdatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockInventoryUpdatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.InventoryId,
            domainEvent.VariantId,
            domainEvent.TotalQuantity);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
