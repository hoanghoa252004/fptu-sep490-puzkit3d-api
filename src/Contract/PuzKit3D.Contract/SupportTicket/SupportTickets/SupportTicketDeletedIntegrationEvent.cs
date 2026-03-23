using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.SupportTicket.SupportTickets;

public sealed record SupportTicketDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid SupportTicketId) : IIntegrationEvent;
