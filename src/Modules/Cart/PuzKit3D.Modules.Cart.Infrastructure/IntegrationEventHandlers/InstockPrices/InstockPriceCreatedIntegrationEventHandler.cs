using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockPrices;

internal sealed class InstockPriceCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockPriceCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockPriceCreatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockPriceCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingPrice = await _context.InStockPriceReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.PriceId, cancellationToken);

        if (existingPrice is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = InStockPriceReplica.Create(
            @event.PriceId,
            @event.Name,
            @event.EffectiveFrom,
            @event.EffectiveTo,
            @event.Priority,
            @event.IsActive,
            @event.OccurredOn,
            @event.OccurredOn);

        _context.InStockPriceReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
