using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;

public sealed record SupportTicketStatusChangedDomainEvent(
    Guid SupportTicketId,
    string Status,
    string Type,
    Guid OrderId,
    DateTime UpdatedAt) : DomainEvent;
