using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;

public sealed record SupportTicketCreatedDomainEvent(
    Guid SupportTicketId,
    Guid UserId,
    Guid OrderId,
    string Type,
    string Status,
    string Reason,
    string Proof,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<SupportTicketDetailInfo> Details) : DomainEvent;

public sealed record SupportTicketDetailInfo(
    Guid SupportTicketDetailId,
    Guid OrderItemId,
    Guid? PartId,
    int Quantity,
    string? Note);
