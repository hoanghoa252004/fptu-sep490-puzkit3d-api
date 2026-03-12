using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InStock;

internal sealed class InstockPriceChangedIntegrationEventHandler 
    : IIntegrationEventHandler<InstockPriceChangedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockPriceChangedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockPriceChangedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.InStockPriceReplicas
            .FirstOrDefaultAsync(r => r.Id == @event.PriceId, cancellationToken);

        if (replica is null)
        {
            // Create new
            replica = InStockPriceReplica.Create(
                @event.PriceId,
                @event.Name,
                @event.EffectiveFrom,
                @event.EffectiveTo,
                @event.Priority,
                @event.IsActive,
                @event.OccurredOn,
                @event.OccurredOn);

            _context.InStockPriceReplicas.Add(replica);
        }
        else
        {
            // Update existing
            var updatedReplica = InStockPriceReplica.Create(
                @event.PriceId,
                @event.Name,
                @event.EffectiveFrom,
                @event.EffectiveTo,
                @event.Priority,
                @event.IsActive,
                replica.CreatedAt,
                @event.OccurredOn);

            _context.Entry(replica).CurrentValues.SetValues(updatedReplica);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
