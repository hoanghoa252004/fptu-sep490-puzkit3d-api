using PuzKit3D.Contract.Delivery;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.Wallet;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.DeliveryTrackings;

public sealed class OrderReturnedIntegrationEventHandler : IIntegrationEventHandler<OrderReturnedIntegrationEvent>
{
    private readonly IInstockOrderRepository _instockOrderRepository;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly IInstockInventoryRepository _inventoryRepository;
    private readonly IEventBus _eventBus;

    public OrderReturnedIntegrationEventHandler(
        IInstockOrderRepository instockOrderRepository,
        IInstockInventoryRepository inventoryRepository,
        IInStockUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _instockOrderRepository = instockOrderRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _inventoryRepository = inventoryRepository;
    }

    public async Task HandleAsync(
        OrderReturnedIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        // Find the instock order
        var instockOrderId = InstockOrderId.From(@event.OrderId);
        var instockOrder = await _instockOrderRepository.GetByIdWithDetailsAsync(instockOrderId, cancellationToken);

        if (instockOrder == null)
            return;

        // Update order status to Returned
        var markAsReturnedResult = instockOrder.MarkAsReturned();
        
        if (markAsReturnedResult.IsFailure)
            return;

        // Increase stock quantity for each returned product
        foreach (var item in instockOrder.OrderDetails)
        {
            var inventory = await _inventoryRepository.GetByVariantIdAsync(
                item.InstockProductVariantId.Value,
                cancellationToken);

            if (inventory is not null)
            {
                // Add back the quantity to reverse the stock deduction
                var addStockResult = inventory.AddStock(item.Quantity);
                if (addStockResult.IsSuccess)
                {
                    _inventoryRepository.Update(inventory);

                    // Publish integration event for inventory update
                    var inventoryUpdatedEvent = new InstockInventoryUpdatedIntegrationEvent(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        inventory.Id.Value,
                        inventory.InstockProductVariantId.Value,
                        inventory.TotalQuantity);

                    await _eventBus.PublishAsync(inventoryUpdatedEvent, cancellationToken);
                }
            }
        }

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
