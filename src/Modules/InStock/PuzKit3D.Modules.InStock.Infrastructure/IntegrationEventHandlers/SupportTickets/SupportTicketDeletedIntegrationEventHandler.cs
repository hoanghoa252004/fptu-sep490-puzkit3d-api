using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketDeletedIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketDeletedIntegrationEvent>
{
    private readonly InStockDbContext _dbContext;

    public SupportTicketDeletedIntegrationEventHandler(
        InStockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        SupportTicketDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _dbContext.SupportTicketReplicas
            .FirstOrDefaultAsync(st => st.Id == @event.SupportTicketId, cancellationToken);

        if (replica == null)
            return;

        _dbContext.SupportTicketReplicas.Remove(replica);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
