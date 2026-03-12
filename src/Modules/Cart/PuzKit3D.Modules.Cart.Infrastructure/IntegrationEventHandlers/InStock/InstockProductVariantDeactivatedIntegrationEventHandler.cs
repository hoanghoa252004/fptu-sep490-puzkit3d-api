using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InStock;

internal sealed class InstockProductVariantDeactivatedIntegrationEventHandler 
    : IIntegrationEventHandler<InstockProductVariantDeactivatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductVariantDeactivatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductVariantDeactivatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.InStockProductVariantReplicas
            .FirstOrDefaultAsync(r => r.Id == @event.VariantId, cancellationToken);

        if (replica is not null)
        {
            var updatedReplica = Domain.Entities.Replicas.InStockProductVariantReplica.Create(
                replica.Id,
                replica.InStockProductId,
                replica.Sku,
                replica.Color,
                replica.AssembledLengthMm,
                replica.AssembledWidthMm,
                replica.AssembledHeightMm,
                false,
                replica.CreatedAt,
                @event.OccurredOn);

            _context.Entry(replica).CurrentValues.SetValues(updatedReplica);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
