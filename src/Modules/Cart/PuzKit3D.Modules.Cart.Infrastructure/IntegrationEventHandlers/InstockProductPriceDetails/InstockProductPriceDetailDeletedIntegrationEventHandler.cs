using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

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

        // Soft delete: set IsActive = false instead of hard delete
        priceDetail.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
