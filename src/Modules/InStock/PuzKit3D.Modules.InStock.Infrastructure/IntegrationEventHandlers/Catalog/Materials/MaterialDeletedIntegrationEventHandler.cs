using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Materials;

internal sealed class MaterialDeletedIntegrationEventHandler
    : IIntegrationEventHandler<MaterialDeletedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public MaterialDeletedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        MaterialDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.MaterialReplicas
            .FirstOrDefaultAsync(m => m.Id == @event.MaterialId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        // Check if any InstockProduct is using this Material
        var hasProducts = await _context.InstockProducts
            .AnyAsync(p => p.MaterialId == @event.MaterialId, cancellationToken);

        if (hasProducts)
        {
            throw new PuzKit3DException("This material can be delete because there is one or more products belongs");
        }

        _context.MaterialReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

