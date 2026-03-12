using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockPrices;

internal sealed class InstockPriceDeletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockPriceDeletedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockPriceDeletedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockPriceDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var price = await _context.InStockPriceReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.PriceId, cancellationToken);

        if (price is null)
        {
            // Price not found, skip
            return;
        }

        // Soft delete: set IsActive = false instead of hard delete
        price.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
