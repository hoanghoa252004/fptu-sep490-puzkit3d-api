using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using Microsoft.EntityFrameworkCore;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.Parts;

internal class PartDeletedIntegrationEventHandler : IIntegrationEventHandler<PartDeletedIntegrationEvent>
{
    private readonly SupportTicketDbContext _dbContext;
    public PartDeletedIntegrationEventHandler(
        SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task HandleAsync(PartDeletedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var partReplica = await _dbContext.PartReplicas
            .FirstOrDefaultAsync(p => p.PartId == @event.PartId, cancellationToken);

        if (partReplica != null)
        {
            _dbContext.PartReplicas.Remove(partReplica);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
