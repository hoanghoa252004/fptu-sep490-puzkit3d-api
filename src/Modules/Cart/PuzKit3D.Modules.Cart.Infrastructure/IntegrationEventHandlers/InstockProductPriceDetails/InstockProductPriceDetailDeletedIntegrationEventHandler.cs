using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductPriceDetails;

internal sealed class InstockProductPriceDetailDeletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductPriceDetailDeletedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductPriceDetailDeletedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductPriceDetailDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var priceDetail = await _context.InStockProductPriceDetailReplicas
            .FirstOrDefaultAsync(pd => pd.Id == @event.PriceDetailId, cancellationToken);

        if (priceDetail is null)
        {
            // Price detail not found, skip
            return;
        }

        // Check if any cart items are using this price detail
        var hasCartItems = await _context.CartItems
            .AnyAsync(ci => ci.InStockProductPriceDetailId == @event.PriceDetailId, cancellationToken);

        if (hasCartItems)
        {
            throw new PuzKit3DException("This price has been applied and cannot be deleted");
        }

        // Delete price detail replica
        _context.InStockProductPriceDetailReplicas.Remove(priceDetail);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

