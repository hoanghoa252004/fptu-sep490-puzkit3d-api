using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductPriceDetails;

internal sealed class InstockProductPriceDetailCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductPriceDetailCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductPriceDetailCreatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductPriceDetailCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingDetail = await _context.InStockProductPriceDetailReplicas
            .FirstOrDefaultAsync(pd => pd.Id == @event.PriceDetailId, cancellationToken);

        if (existingDetail is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = InStockProductPriceDetailReplica.Create(
            @event.PriceDetailId,
            @event.PriceId,
            @event.VariantId,
            @event.UnitPrice,
            @event.IsActive,
            @event.OccurredOn,
            @event.OccurredOn);

        _context.InStockProductPriceDetailReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
