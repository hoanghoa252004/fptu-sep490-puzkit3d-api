using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.Parts;

internal class PartUpdatedIntegrationEventHandler : IIntegrationEventHandler<PartUpdatedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;
    public PartUpdatedIntegrationEventHandler(
        DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task HandleAsync(PartUpdatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var partReplica = await _dbContext.PartReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.PartId, cancellationToken);

        if (partReplica != null)
        {
            partReplica.Update(
                @event.Name,
                @event.PartType,
                @event.Code,
                @event.Quantity);

            _dbContext.PartReplicas.Update(partReplica);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
