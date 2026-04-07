using MediatR;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.Payments;

internal sealed class OrderExpiredToDoPaymentIntegrationEventHandler : IIntegrationEventHandler<OrderExpiredToDoPaymentIntegrationEvent>
{
    private readonly IPartnerProductOrderRepository _orderRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public OrderExpiredToDoPaymentIntegrationEventHandler(
        IPartnerProductOrderRepository orderRepository,
        IPartnerUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(
        OrderExpiredToDoPaymentIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Get the order by ID
        var order = await _orderRepository.GetByIdAsync(
            PartnerProductOrderId.From(@event.OrderId),
            cancellationToken);

        if (order == null)
            return;

        // Update order status to Expired
        var markAsExpiredResult = order.UpdateStatus(PartnerProductOrderStatus.Expired);

        if (markAsExpiredResult.IsFailure)
            return;

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish InstockOrderStatusChangedIntegrationEvent
        var integrationEvent = new PartnerProductOrderStatusUpdatedIntegrationEvent(
            Guid.NewGuid(),
            @event.OccurredOn,
            @event.OrderId,
            order.Code,
            order.CustomerId,
            PartnerProductOrderStatus.Expired.ToString(),
            DateTime.UtcNow);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}