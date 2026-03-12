using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InStock;

internal sealed class InstockInventoryChangedIntegrationEventHandler 
    : IIntegrationEventHandler<InstockInventoryChangedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockInventoryChangedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockInventoryChangedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.InStockInventoryReplicas
            .FirstOrDefaultAsync(r => r.Id == @event.InventoryId, cancellationToken);

        if (replica is null)
        {
            // Create new
            replica = InStockInventoryReplica.Create(
                @event.InventoryId,
                @event.VariantId,
                @event.TotalQuantity,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockInventoryReplicas.Add(replica);
        }
        else
        {
            // Update quantity
            replica.UpdateQuantity(@event.TotalQuantity);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
