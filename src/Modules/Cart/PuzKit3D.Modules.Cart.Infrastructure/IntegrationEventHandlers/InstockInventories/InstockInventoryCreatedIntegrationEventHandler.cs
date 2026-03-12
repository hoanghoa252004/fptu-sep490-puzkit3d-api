using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockInventories;

internal sealed class InstockInventoryCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockInventoryCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockInventoryCreatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockInventoryCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingInventory = await _context.InStockInventoryReplicas
            .FirstOrDefaultAsync(i => i.Id == @event.InventoryId, cancellationToken);

        if (existingInventory is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = InStockInventoryReplica.Create(
            @event.InventoryId,
            @event.VariantId,
            @event.TotalQuantity,
            @event.OccurredOn,
            @event.OccurredOn);

        _context.InStockInventoryReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
