using MediatR;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.PartnerProductOrders;

internal class PartnerProductOrderCreatedDomainEventHandler
    : INotificationHandler<PartnerProductOrderCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    public PartnerProductOrderCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public async Task Handle(PartnerProductOrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var orderDetails = new List<PartnerProductOrderItem>();

        foreach (var detail in notification.Items)
        {
            orderDetails.Add(new PartnerProductOrderItem(
                detail.OrderDetailId,
                detail.PartnerProductId,
                detail.Quantity,
                detail.ProductName
                ));
        }

        var integrationEvent = new PartnerProductOrderCreatedIntegrationEvent
            (
                Guid.NewGuid(),
                notification.OccurredOn,
                notification.OrderId,
                notification.CustomerId,
                notification.Code,
                notification.GrandTotalAmount,
                notification.Status.ToString(),
                notification.PaymentMethod,
                notification.UsedCoinAmount,
                notification.IsPaid,
                notification.PaidAt,
                notification.CreatedAt,
                orderDetails
            );

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
