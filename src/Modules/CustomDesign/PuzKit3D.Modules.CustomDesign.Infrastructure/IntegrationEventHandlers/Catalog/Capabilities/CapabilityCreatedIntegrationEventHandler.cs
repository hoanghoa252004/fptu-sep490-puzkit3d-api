using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Capabilities;

internal sealed class CapabilityCreatedIntegrationEventHandler
    : IIntegrationEventHandler<CapabilityCreatedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public CapabilityCreatedIntegrationEventHandler(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        CapabilityCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingReplica = await _context.CapabilityReplicas
            .FirstOrDefaultAsync(c => c.Id == @event.CapabilityId, cancellationToken);

        if (existingReplica is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = CapabilityReplica.Create(
            @event.CapabilityId,
            @event.Name,
            @event.Slug,
            @event.Description,
            @event.IsActive,
            @event.CreatedAt,
            @event.CreatedAt);

        _context.CapabilityReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
