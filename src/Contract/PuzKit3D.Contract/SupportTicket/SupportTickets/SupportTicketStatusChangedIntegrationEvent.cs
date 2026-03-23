using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.SupportTicket.SupportTickets;

public sealed record SupportTicketStatusChangedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid SupportTicketId,
    string Status,
    DateTime UpdatedAt) : IIntegrationEvent;
