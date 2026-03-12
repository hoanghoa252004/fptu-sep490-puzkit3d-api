using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockInventories;

internal sealed class InstockInventoryDeletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockInventoryDeletedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockInventoryDeletedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockInventoryDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var inventory = await _context.InStockInventoryReplicas
            .FirstOrDefaultAsync(i => i.Id == @event.InventoryId, cancellationToken);

        if (inventory is null)
        {
            // Inventory not found, skip
            return;
        }

        _context.InStockInventoryReplicas.Remove(inventory);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
