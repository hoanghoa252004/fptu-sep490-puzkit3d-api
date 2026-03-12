using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

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

        // Soft delete: set IsActive = false instead of hard delete
        variant.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
