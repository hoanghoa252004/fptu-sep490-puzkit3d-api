using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketStatusChangedIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketStatusChangedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;

    public SupportTicketStatusChangedIntegrationEventHandler(
        DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        SupportTicketStatusChangedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _dbContext.SupportTicketReplicas
            .FirstOrDefaultAsync(st => st.Id == @event.SupportTicketId, cancellationToken);

        if (replica == null)
            return;

        replica.UpdateStatus(@event.Status, @event.UpdatedAt);
        _dbContext.SupportTicketReplicas.Update(replica);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
