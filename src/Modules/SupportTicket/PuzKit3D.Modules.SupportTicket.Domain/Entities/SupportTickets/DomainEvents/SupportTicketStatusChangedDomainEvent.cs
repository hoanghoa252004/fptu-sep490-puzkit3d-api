using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;

public sealed record SupportTicketStatusChangedDomainEvent(
    Guid SupportTicketId,
    string Status,
    DateTime UpdatedAt) : DomainEvent;
