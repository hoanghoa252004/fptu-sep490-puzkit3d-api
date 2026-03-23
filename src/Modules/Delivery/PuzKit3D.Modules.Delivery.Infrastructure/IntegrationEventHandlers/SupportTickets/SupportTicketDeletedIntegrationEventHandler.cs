using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketDeletedIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketDeletedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;

    public SupportTicketDeletedIntegrationEventHandler(
        DeliveryDbContext dbContext)
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
