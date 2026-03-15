using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

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

        // Check if any cart items are using price details that belong to this price
        var priceDetailsOfPrice = await _context.InStockProductPriceDetailReplicas
            .Where(pd => pd.InStockPriceId == @event.PriceId)
            .Select(pd => pd.Id)
            .ToListAsync(cancellationToken);

        if (priceDetailsOfPrice.Any())
        {
            var hasCartItems = await _context.CartItems
                .AnyAsync(ci => ci.InStockProductPriceDetailId.HasValue && 
                           priceDetailsOfPrice.Contains(ci.InStockProductPriceDetailId.Value), 
                           cancellationToken);

            if (hasCartItems)
            {
                throw new PuzKit3DException("This price has been applied and cannot be deleted");
            }
        }

        // Delete price replica
        _context.InStockPriceReplicas.Remove(price);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

