using MediatR;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Application.DomainEventHandlers.SupportTickets;

internal sealed class SupportTicketCreatedDomainEventHandler
    : INotificationHandler<SupportTicketCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public SupportTicketCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        SupportTicketCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new SupportTicketCreatedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            notification.SupportTicketId,
            notification.Code,
            notification.UserId,
            notification.OrderId,
            notification.Type,
            notification.Status,
            notification.Reason,
            notification.Proof,
            notification.CreatedAt,
            notification.UpdatedAt,
            notification.Details
                .Select(d => new global::PuzKit3D.Contract.SupportTicket.SupportTickets.SupportTicketDetailInfo(
                    d.SupportTicketDetailId,
                    d.OrderItemId,
                    d.PartId,
                    d.Quantity,
                    d.Note))
                .ToList());

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
