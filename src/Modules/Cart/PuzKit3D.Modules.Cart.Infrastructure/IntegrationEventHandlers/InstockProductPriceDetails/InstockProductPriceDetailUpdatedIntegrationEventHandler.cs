using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductPriceDetails;

internal sealed class InstockProductPriceDetailUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductPriceDetailUpdatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductPriceDetailUpdatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductPriceDetailUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var priceDetail = await _context.InStockProductPriceDetailReplicas
            .FirstOrDefaultAsync(pd => pd.Id == @event.PriceDetailId, cancellationToken);

        if (priceDetail is null)
        {
            // Create if not exists
            priceDetail = InStockProductPriceDetailReplica.Create(
                @event.PriceDetailId,
                @event.PriceId,
                @event.VariantId,
                @event.UnitPrice,
                @event.IsActive,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockProductPriceDetailReplicas.Add(priceDetail);
        }
        else
        {
            // Update existing
            var updated = InStockProductPriceDetailReplica.Create(
                @event.PriceDetailId,
                @event.PriceId,
                @event.VariantId,
                @event.UnitPrice,
                @event.IsActive,
                priceDetail.CreatedAt,
                @event.OccurredOn);

            _context.Entry(priceDetail).CurrentValues.SetValues(updated);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
