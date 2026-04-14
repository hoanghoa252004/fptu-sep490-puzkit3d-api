using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketExchangeProcessingIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketExchangeProcessingIntegrationEvent>
{
    private readonly CartDbContext _context;

    public SupportTicketExchangeProcessingIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        SupportTicketExchangeProcessingIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var item in @event.Items)
            {
                // Get inventory replica by variant ID
                var inventory = await _context.InStockInventoryReplicas
                    .FirstOrDefaultAsync(i => i.InStockProductVariantId == item.VariantId, cancellationToken);

                if (inventory is null)
                {
                    // Inventory not found, skip
                    continue;
                }

                // Reduce stock quantity for exchange
                var newQuantity = inventory.TotalQuantity - item.Quantity;
                if (newQuantity >= 0)
                {
                    inventory.UpdateQuantity(newQuantity);
                    _context.InStockInventoryReplicas.Update(inventory);
                }

                else
                {
                    throw new PuzKit3DException($"Insufficient stock for variant {item.VariantId} during exchange processing.");
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Log error or handle appropriately
            throw;
        }
    }
}
