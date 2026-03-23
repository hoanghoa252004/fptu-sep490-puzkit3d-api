using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;
using Microsoft.EntityFrameworkCore;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.Parts;

internal class PartUpdatedIntegrationEventHandler : IIntegrationEventHandler<PartUpdatedIntegrationEvent>
{
    private readonly SupportTicketDbContext _dbContext;
    public PartUpdatedIntegrationEventHandler(
        SupportTicketDbContext dbContext)
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
