using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Drives;

internal sealed class DriveUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<DriveUpdatedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public DriveUpdatedIntegrationEventHandler(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        DriveUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.DriveReplicas
            .FirstOrDefaultAsync(d => d.Id == @event.DriveId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        replica.Update(
            @event.Name,
            @event.Description,
            @event.MinVolume,
            @event.QuantityInStock,
            @event.IsActive,
            @event.UpdatedAt);

        _context.DriveReplicas.Update(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
