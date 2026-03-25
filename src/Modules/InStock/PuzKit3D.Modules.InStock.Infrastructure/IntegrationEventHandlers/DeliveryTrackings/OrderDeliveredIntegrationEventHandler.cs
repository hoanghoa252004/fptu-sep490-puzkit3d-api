using PuzKit3D.Contract.Delivery;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.DeliveryTrackings;

public sealed class OrderDeliveredIntegrationEventHandler : IIntegrationEventHandler<OrderDeliveredIntegrationEvent>
{
    private readonly IInstockOrderRepository _instockOrderRepository;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public OrderDeliveredIntegrationEventHandler(
        IInstockOrderRepository instockOrderRepository,
        IInStockUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _instockOrderRepository = instockOrderRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(
        OrderDeliveredIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        // Find the instock order by the delivery tracking's order ID
        var instockOrderId = InstockOrderId.From(@event.OrderId);
        var instockOrder = await _instockOrderRepository.GetByIdAsync(instockOrderId, cancellationToken);

        if (instockOrder == null)
            return;

        // If status is Delivered, set IsPaid = true and PaidAt = now
        if (instockOrder.Status == InstockOrderStatus.HandedOverToDelivery)
        {
            var markAsPaidResult = instockOrder.MarkAsPaid(DateTime.UtcNow);
            
            if (markAsPaidResult.IsFailure)
                return;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}


