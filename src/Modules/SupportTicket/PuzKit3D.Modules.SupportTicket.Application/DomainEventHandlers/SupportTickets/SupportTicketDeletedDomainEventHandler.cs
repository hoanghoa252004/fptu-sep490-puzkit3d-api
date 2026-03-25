using MediatR;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Application.DomainEventHandlers.SupportTickets;

internal sealed class SupportTicketDeletedDomainEventHandler
    : INotificationHandler<SupportTicketDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public SupportTicketDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        SupportTicketDeletedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new SupportTicketDeletedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            notification.SupportTicketId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
