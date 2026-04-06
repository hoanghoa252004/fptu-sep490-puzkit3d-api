using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Capabilities;

internal sealed class CapabilityDeletedIntegrationEventHandler
    : IIntegrationEventHandler<CapabilityDeletedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public CapabilityDeletedIntegrationEventHandler(CustomDesignDbContext context)
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

        _context.CapabilityReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
