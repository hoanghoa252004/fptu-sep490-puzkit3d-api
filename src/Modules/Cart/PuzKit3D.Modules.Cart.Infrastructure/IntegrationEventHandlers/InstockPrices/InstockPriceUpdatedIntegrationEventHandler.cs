using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockPrices;

internal sealed class InstockPriceUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockPriceUpdatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockPriceUpdatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockPriceUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var price = await _context.InStockPriceReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.PriceId, cancellationToken);

        if (price is null)
        {
            // Create if not exists
            price = InStockPriceReplica.Create(
                @event.PriceId,
                @event.Name,
                @event.EffectiveFrom,
                @event.EffectiveTo,
                @event.Priority,
                @event.IsActive,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockPriceReplicas.Add(price);
        }
        else
        {
            // Update existing
            var updated = InStockPriceReplica.Create(
                @event.PriceId,
                @event.Name,
                @event.EffectiveFrom,
                @event.EffectiveTo,
                @event.Priority,
                @event.IsActive,
                price.CreatedAt,
                @event.OccurredOn);

            _context.Entry(price).CurrentValues.SetValues(updated);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
