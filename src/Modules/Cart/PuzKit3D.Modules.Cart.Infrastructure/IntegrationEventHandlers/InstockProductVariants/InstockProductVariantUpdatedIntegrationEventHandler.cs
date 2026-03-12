using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductVariants;

internal sealed class InstockProductVariantUpdatedIntegrationEventHandler 
    : IIntegrationEventHandler<InstockProductVariantUpdatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductVariantUpdatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductVariantUpdatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.InStockProductVariantReplicas
            .FirstOrDefaultAsync(r => r.Id == @event.VariantId, cancellationToken);

        if (replica is null)
        {
            // Create if not exists (shouldn't happen but handle it)
            replica = InStockProductVariantReplica.Create(
                @event.VariantId,
                @event.ProductId,
                @event.Sku,
                @event.Color,
                @event.AssembledLengthMm,
                @event.AssembledWidthMm,
                @event.AssembledHeightMm,
                @event.IsActive,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockProductVariantReplicas.Add(replica);
        }
        else
        {
            // Update existing
            var updatedReplica = InStockProductVariantReplica.Create(
                @event.VariantId,
                @event.ProductId,
                @event.Sku,
                @event.Color,
                @event.AssembledLengthMm,
                @event.AssembledWidthMm,
                @event.AssembledHeightMm,
                @event.IsActive,
                replica.CreatedAt,
                @event.OccurredOn);

            _context.Entry(replica).CurrentValues.SetValues(updatedReplica);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
