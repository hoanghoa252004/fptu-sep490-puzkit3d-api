using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketCreatedIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketCreatedIntegrationEvent>
{
    private readonly InStockDbContext _dbContext;

    public SupportTicketCreatedIntegrationEventHandler(
        InStockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        SupportTicketCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = SupportTicketReplica.Create(
            @event.SupportTicketId,
            @event.Code,
            @event.UserId,
            @event.OrderId,
            @event.Type,
            @event.Status,
            @event.Reason,
            @event.Proof,
            @event.CreatedAt,
            @event.UpdatedAt);

        await _dbContext.SupportTicketReplicas.AddAsync(replica, cancellationToken);

        foreach (var detail in @event.Details)
        {
            var detailReplica = SupportTicketDetailReplica.Create(
                detail.SupportTicketDetailId,
                @event.SupportTicketId,
                detail.OrderItemId,
                detail.DriveId,
                detail.Quantity,
                detail.Note);

            await _dbContext.SupportTicketDetailReplicas.AddAsync(detailReplica, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
