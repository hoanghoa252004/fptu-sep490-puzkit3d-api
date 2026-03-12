using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProducts;

internal sealed class InstockProductDeletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductDeletedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductDeletedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var product = await _context.InStockProductReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.ProductId, cancellationToken);

        if (product is null)
        {
            // Product not found, skip
            return;
        }

        // Soft delete: set IsActive = false instead of hard delete
        product.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
