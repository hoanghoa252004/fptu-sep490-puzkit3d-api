using PuzKit3D.Contract.Delivery;
using PuzKit3D.Contract.Wallet;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.DeliveryTrackings;

public sealed class OrderReturnedIntegrationEventHandler : IIntegrationEventHandler<OrderReturnedIntegrationEvent>
{
    private readonly IInstockOrderRepository _instockOrderRepository;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public OrderReturnedIntegrationEventHandler(
        IInstockOrderRepository instockOrderRepository,
        IInStockUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _instockOrderRepository = instockOrderRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(
        OrderReturnedIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        // Find the instock order
        var instockOrderId = InstockOrderId.From(@event.OrderId);
        var instockOrder = await _instockOrderRepository.GetByIdAsync(instockOrderId, cancellationToken);

        if (instockOrder == null)
            return;

        // Update order status to Returned
        var markAsReturnedResult = instockOrder.MarkAsReturned();
        
        if (markAsReturnedResult.IsFailure)
            return;
        // AHIHI
        // Calculate refund amount: 80% of grand total
        //var refundAmount = instockOrder.GrandTotalAmount * 0.8m;

        // Publish refund coin event to Wallet module
        await _eventBus.PublishAsync(
            new OrderReturnRefundCoinIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                @event.OrderId,
                instockOrder.GrandTotalAmount,
                instockOrder.UsedCoinAmount,
                instockOrder.PaymentMethod,
                instockOrder.ShippingFee,
                instockOrder.CustomerId),
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
