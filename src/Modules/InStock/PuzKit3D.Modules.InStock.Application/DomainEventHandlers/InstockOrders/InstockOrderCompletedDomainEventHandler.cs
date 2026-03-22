using MediatR;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrders;

internal sealed class InstockOrderCompletedDomainEventHandler
    : INotificationHandler<InstockOrderCompletedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IInstockProductVariantRepository _variantRepository;

    public InstockOrderCompletedDomainEventHandler(
        IEventBus eventBus,
        IInstockProductVariantRepository variantRepository)
    {
        _eventBus = eventBus;
        _variantRepository = variantRepository;
    }

    public async Task Handle(InstockOrderCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var orderDetails = new List<OrderDetailCompletedInfo>();

        foreach (var detail in notification.OrderDetails)
        {
            var variant = await _variantRepository.GetByIdAsync(
                InstockProductVariantId.From(detail.VariantId),
                cancellationToken);

            var productId = variant?.InstockProductId.Value;

            orderDetails.Add(new OrderDetailCompletedInfo(
                detail.OrderDetailId,
                detail.VariantId,
                productId!.Value,
                detail.Quantity));
        }

        var integrationEvent = new InstockOrderCompletedIntegrationEvent(
            notification.OrderId,
            notification.OccurredOn,
            notification.OrderId,
            notification.Code,
            notification.CustomerId,
            orderDetails,
            notification.CompletedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
