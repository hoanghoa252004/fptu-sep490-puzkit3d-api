using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Events.InstockInventories;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockInventories;

internal sealed class InstockInventoryCreatedDomainEventHandler 
    : INotificationHandler<InstockInventoryCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockInventoryCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockInventoryCreatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockInventoryCreatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.InventoryId,
            domainEvent.VariantId,
            domainEvent.TotalQuantity);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
