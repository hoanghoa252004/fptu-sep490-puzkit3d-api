using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Capabilities;

internal sealed class CapabilityUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<CapabilityUpdatedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public CapabilityUpdatedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        CapabilityUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.CapabilityReplicas
            .FirstOrDefaultAsync(c => c.Id == @event.CapabilityId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        replica.Update(
            @event.Name,
            @event.Slug,
            @event.FactorPercentage,
            @event.Description,
            @event.IsActive,
            @event.UpdatedAt);

        _context.CapabilityReplicas.Update(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
