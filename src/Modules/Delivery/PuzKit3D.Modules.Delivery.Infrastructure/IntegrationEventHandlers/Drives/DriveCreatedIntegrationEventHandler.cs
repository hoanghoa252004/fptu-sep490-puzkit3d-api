using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.Drives;

internal sealed class DriveCreatedIntegrationEventHandler
    : IIntegrationEventHandler<DriveCreatedIntegrationEvent>
{
    private readonly DeliveryDbContext _context;

    public DriveCreatedIntegrationEventHandler(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        DriveCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingReplica = await _context.DriveReplicas
            .FirstOrDefaultAsync(d => d.Id == @event.DriveId, cancellationToken);

        if (existingReplica is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = DriveReplica.Create(
            @event.DriveId,
            @event.Name,
            @event.Description,
            @event.MinVolume,
            @event.QuantityInStock,
            @event.IsActive,
            @event.CreatedAt,
            @event.CreatedAt);

        _context.DriveReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
