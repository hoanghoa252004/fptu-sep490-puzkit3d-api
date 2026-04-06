using MediatR;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.PartnerProductOrders;

internal record class PartnerProductOrderRefundedDomainEventHandler
    : INotificationHandler<PartnerProductOrderRefundedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartnerProductOrderRefundedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        PartnerProductOrderRefundedDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new PartnerProductOrderRefundedIntegrationEvent(
            notification.Id,
            notification.OccurredOn,
            notification.OrderId,
            notification.Code,
            notification.CustomerId,
            notification.GrandTotalAmount,
            notification.UserCoinAmount,
            notification.PaymentMethod,
            notification.PercentRefund,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
