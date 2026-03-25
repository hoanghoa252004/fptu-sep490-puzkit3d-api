using MediatR;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrders;

internal sealed class InstockOrderCreatedDomainEventHandler
    : INotificationHandler<InstockOrderCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IInstockProductVariantRepository _variantRepository;

    public InstockOrderCreatedDomainEventHandler(IEventBus eventBus,
        IInstockProductVariantRepository variantRepository)
    {
        _eventBus = eventBus;
        _variantRepository = variantRepository;
    }

    public async Task Handle(InstockOrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var orderDetails = new List<OrderDetail>();

        foreach (var detail in notification.OrderDetails)
        {
            var variant = await _variantRepository.GetByIdAsync(
                InstockProductVariantId.From(detail.VariantId),
                cancellationToken);

            var productId = variant?.InstockProductId.Value;

            orderDetails.Add(new OrderDetail(
                detail.OrderDetailId,
                detail.VariantId,
                productId!.Value,
                detail.Quantity,
                detail.ProductName,
                detail.VariantName));
        }

        var integrationEvent = new InstockOrderCreatedIntegrationEvent(
            notification.Id,
            notification.OccurredOn,
            notification.OrderId,
            notification.CustomerId,
            notification.CartItemIds,
            notification.Code,
            notification.GrandTotalAmount,
            notification.Status,
            notification.PaymentMethod,
            notification.IsPaid,
            notification.PaidAt,
            notification.CreatedAt,
            orderDetails);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
