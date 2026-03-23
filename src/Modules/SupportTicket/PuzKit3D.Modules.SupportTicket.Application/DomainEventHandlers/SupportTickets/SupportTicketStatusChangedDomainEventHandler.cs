using MediatR;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Application.DomainEventHandlers.SupportTickets;

internal sealed class SupportTicketStatusChangedDomainEventHandler
    : INotificationHandler<SupportTicketStatusChangedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public SupportTicketStatusChangedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        SupportTicketStatusChangedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new SupportTicketStatusChangedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            notification.SupportTicketId,
            notification.Status,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
