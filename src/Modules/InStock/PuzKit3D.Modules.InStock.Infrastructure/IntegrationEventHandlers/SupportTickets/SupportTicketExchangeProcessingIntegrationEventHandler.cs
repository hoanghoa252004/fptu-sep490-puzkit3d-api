using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketExchangeProcessingIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketExchangeProcessingIntegrationEvent>
{
    private readonly InStockDbContext _dbContext;
    private readonly IInstockInventoryRepository _inventoryRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public SupportTicketExchangeProcessingIntegrationEventHandler(
        InStockDbContext dbContext,
        IInstockInventoryRepository inventoryRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        SupportTicketExchangeProcessingIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var item in @event.Items)
            {
                // Get inventory by variant ID
                var inventory = await _inventoryRepository.GetByVariantIdAsync(item.VariantId, cancellationToken);

                if (inventory is null)
                {
                    // Inventory not found, skip
                    continue;
                }

                // Reduce stock quantity

                var newQuantity = inventory.TotalQuantity - item.Quantity;
                if (newQuantity >= 0)
                {
                    var result = inventory.ReduceStock(item.Quantity);
                    if (result.IsSuccess)
                    {
                        _inventoryRepository.Update(inventory);
                    }
                }

                else
                {
                    throw new PuzKit3DException($"Insufficient stock for variant {item.VariantId} during exchange processing.");
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Log error or handle appropriately
            throw;
        }
    }
}
