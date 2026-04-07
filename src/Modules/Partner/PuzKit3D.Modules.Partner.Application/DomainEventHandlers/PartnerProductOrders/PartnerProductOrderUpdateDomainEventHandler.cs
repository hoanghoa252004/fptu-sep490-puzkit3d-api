using MediatR;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.PartnerProductOrders;

internal class PartnerProductOrderUpdateDomainEventHandler
    : INotificationHandler<PartnerProductOrderStatusUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    public PartnerProductOrderUpdateDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public async Task Handle(PartnerProductOrderStatusUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartnerProductOrderStatusUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.OrderId,
            notification.Code,
            notification.CustomerId,
            notification.NewStatus.ToString(),
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
