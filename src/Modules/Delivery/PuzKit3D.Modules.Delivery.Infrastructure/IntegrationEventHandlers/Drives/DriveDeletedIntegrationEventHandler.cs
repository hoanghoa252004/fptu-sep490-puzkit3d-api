using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.Drives;

internal sealed class DriveDeletedIntegrationEventHandler
    : IIntegrationEventHandler<DriveDeletedIntegrationEvent>
{
    private readonly DeliveryDbContext _context;

    public DriveDeletedIntegrationEventHandler(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        DriveDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.DriveReplicas
            .FirstOrDefaultAsync(d => d.Id == @event.DriveId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        _context.DriveReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
