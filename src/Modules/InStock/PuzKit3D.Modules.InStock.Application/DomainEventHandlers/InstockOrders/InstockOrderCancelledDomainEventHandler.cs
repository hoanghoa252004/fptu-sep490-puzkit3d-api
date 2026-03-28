using MediatR;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrders;

internal sealed class InstockOrderCancelledDomainEventHandler
    : INotificationHandler<InstockOrderCancelledDomainEvent>
{
    private readonly IInstockInventoryRepository _inventoryRepository;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public InstockOrderCancelledDomainEventHandler(
        IInstockInventoryRepository inventoryRepository,
        IInStockUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockOrderCancelledDomainEvent notification,
        CancellationToken cancellationToken)
    {
        // Reverse inventory for each order detail in InStock module
        foreach (var orderDetail in notification.OrderDetails)
        {
            var inventory = await _inventoryRepository.GetByVariantIdAsync(
                orderDetail.VariantId,
                cancellationToken);

            if (inventory is not null)
            {
                // Add back the quantity to reverse the stock deduction
                var addStockResult = inventory.AddStock(orderDetail.Quantity);
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
