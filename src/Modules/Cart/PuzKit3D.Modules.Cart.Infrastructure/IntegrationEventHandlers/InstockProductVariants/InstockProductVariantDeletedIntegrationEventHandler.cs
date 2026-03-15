using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductVariants;

internal sealed class InstockProductVariantDeletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductVariantDeletedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductVariantDeletedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductVariantDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var variant = await _context.InStockProductVariantReplicas
            .FirstOrDefaultAsync(v => v.Id == @event.VariantId, cancellationToken);

        if (variant is null)
        {
            // Variant not found, skip
            return;
        }

        // Check if any cart items are using this variant
        var hasCartItems = await _context.CartItems
            .AnyAsync(ci => ci.ItemId == @event.VariantId, cancellationToken);

        if (hasCartItems)
        {
            throw new PuzKit3DException("Someone has added this item to their cart");
        }

        // Delete variant replica
        _context.InStockProductVariantReplicas.Remove(variant);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

