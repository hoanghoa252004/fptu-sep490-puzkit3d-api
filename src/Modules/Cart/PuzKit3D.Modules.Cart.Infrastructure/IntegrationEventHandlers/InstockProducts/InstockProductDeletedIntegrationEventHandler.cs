using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

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

        // Check if any cart items are using variants of this product
        var variantsOfProduct = await _context.InStockProductVariantReplicas
            .Where(v => v.InStockProductId == @event.ProductId)
            .Select(v => v.Id)
            .ToListAsync(cancellationToken);

        if (variantsOfProduct.Any())
        {
            var hasCartItems = await _context.CartItems
                .AnyAsync(ci => variantsOfProduct.Contains(ci.ItemId), cancellationToken);

            if (hasCartItems)
            {
                throw new PuzKit3DException("Someone has added this item to their cart");
            }
        }

        // Delete product replica
        _context.InStockProductReplicas.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

