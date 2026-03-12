using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockInventories;

internal sealed class InstockInventoryUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockInventoryUpdatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockInventoryUpdatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockInventoryUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var inventory = await _context.InStockInventoryReplicas
            .FirstOrDefaultAsync(i => i.Id == @event.InventoryId, cancellationToken);

        if (inventory is null)
        {
            // Create if not exists
            inventory = InStockInventoryReplica.Create(
                @event.InventoryId,
                @event.VariantId,
                @event.TotalQuantity,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockInventoryReplicas.Add(inventory);
        }
        else
        {
            // Update existing
            var updated = InStockInventoryReplica.Create(
                @event.InventoryId,
                @event.VariantId,
                @event.TotalQuantity,
                inventory.CreatedAt,
                @event.OccurredOn);

            _context.Entry(inventory).CurrentValues.SetValues(updated);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
