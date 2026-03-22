using MediatR;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails.DomainEvents;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockOrderDetails;

internal sealed class InstockOrderDetailCreatedDomainEventHandler : INotificationHandler<InstockOrderDetailCreatedDomainEvent>
{
    private readonly IInstockInventoryRepository _inventoryRepository;

    public InstockOrderDetailCreatedDomainEventHandler(IInstockInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task Handle(InstockOrderDetailCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var inventory = await _inventoryRepository.GetByVariantIdAsync(notification.VariantId, cancellationToken);

        if (inventory == null)
        {
            // Log warning - inventory not found for variant
            Console.WriteLine($"Warning: Inventory not found for variant {notification.VariantId}. Order detail {notification.OrderDetailId} created but inventory not updated.");
            return;
        }

        var result = inventory.ReduceStock(notification.Quantity);

        if (result.IsFailure)
        {
            // Log error - insufficient inventory
            Console.WriteLine($"Error: Failed to reduce stock for variant {notification.VariantId}. Error: {result.Error.Message}");
            throw new PuzKit3DException($"Failed to reduce stock: {result.Error.Message}");
        }

        _inventoryRepository.Update(inventory);
    }
}
