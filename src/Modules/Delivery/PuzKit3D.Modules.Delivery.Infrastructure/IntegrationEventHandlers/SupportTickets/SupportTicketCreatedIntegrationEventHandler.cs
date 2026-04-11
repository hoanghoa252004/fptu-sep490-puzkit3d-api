using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketCreatedIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketCreatedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;

    public SupportTicketCreatedIntegrationEventHandler(
        DeliveryDbContext dbContext)
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
