using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.Parts;

internal class PartDeletedIntegrationEventHandler : IIntegrationEventHandler<PartDeletedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;
    public PartDeletedIntegrationEventHandler(
        DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task HandleAsync(PartDeletedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var partReplica = await _dbContext.PartReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.PartId, cancellationToken);

        if (partReplica != null)
        {
            _dbContext.PartReplicas.Remove(partReplica);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
