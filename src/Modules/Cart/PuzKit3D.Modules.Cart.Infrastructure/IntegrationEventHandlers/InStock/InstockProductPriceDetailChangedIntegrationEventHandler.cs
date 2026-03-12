using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InStock;

internal sealed class InstockProductPriceDetailChangedIntegrationEventHandler 
    : IIntegrationEventHandler<InstockProductPriceDetailChangedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductPriceDetailChangedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductPriceDetailChangedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.InStockProductPriceDetailReplicas
            .FirstOrDefaultAsync(r => r.Id == @event.PriceDetailId, cancellationToken);

        if (replica is null)
        {
            // Create new
            replica = InStockProductPriceDetailReplica.Create(
                @event.PriceDetailId,
                @event.PriceId,
                @event.VariantId,
                @event.UnitPrice,
                @event.IsActive,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockProductPriceDetailReplicas.Add(replica);
        }
        else
        {
            // Update existing
            var updatedReplica = InStockProductPriceDetailReplica.Create(
                @event.PriceDetailId,
                @event.PriceId,
                @event.VariantId,
                @event.UnitPrice,
                @event.IsActive,
                replica.CreatedAt,
                @event.OccurredOn);

            _context.Entry(replica).CurrentValues.SetValues(updatedReplica);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
