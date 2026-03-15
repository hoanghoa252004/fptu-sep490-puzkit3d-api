using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Capabilities;

internal sealed class CapabilityDeletedIntegrationEventHandler
    : IIntegrationEventHandler<CapabilityDeletedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public CapabilityDeletedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        CapabilityDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.CapabilityReplicas
            .FirstOrDefaultAsync(c => c.Id == @event.CapabilityId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        // Check if any InstockProduct is using this Capability
        var hasProducts = await _context.InstockProducts
            .AnyAsync(p => p.CapabilityId == @event.CapabilityId, cancellationToken);

        if (hasProducts)
        {
            throw new PuzKit3DException("This capability can be delete because there is one or more products belongs");
        }

        _context.CapabilityReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

