using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductVariants;

internal sealed class InstockProductVariantCreatedIntegrationEventHandler 
    : IIntegrationEventHandler<InstockProductVariantCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductVariantCreatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductVariantCreatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        // Check if replica already exists
        var existingReplica = await _context.InStockProductVariantReplicas
            .FirstOrDefaultAsync(r => r.Id == @event.VariantId, cancellationToken);

        if (existingReplica is not null)
        {
            // Update existing
            existingReplica = InStockProductVariantReplica.Create(
                @event.VariantId,
                @event.ProductId,
                @event.Sku,
                @event.Color,
                @event.AssembledLengthMm,
                @event.AssembledWidthMm,
                @event.AssembledHeightMm,
                @event.PreviewImages,
                @event.IsActive,
                existingReplica.CreatedAt,
                @event.OccurredOn);

            _context.InStockProductVariantReplicas.Update(existingReplica);
        }
        else
        {
            // Create new
            var replica = InStockProductVariantReplica.Create(
                @event.VariantId,
                @event.ProductId,
                @event.Sku,
                @event.Color,
                @event.AssembledLengthMm,
                @event.AssembledWidthMm,
                @event.AssembledHeightMm,
                @event.PreviewImages,
                @event.IsActive,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockProductVariantReplicas.Add(replica);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
