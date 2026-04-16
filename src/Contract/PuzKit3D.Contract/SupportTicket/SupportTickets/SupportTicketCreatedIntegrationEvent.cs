using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.SupportTicket.SupportTickets;

public sealed record SupportTicketCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid SupportTicketId,
    string Code,
    Guid UserId,
    Guid OrderId,
    string Type,
    string Status,
    string Reason,
    string Proof,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<SupportTicketDetailInfo> Details) : IIntegrationEvent;

public sealed record SupportTicketDetailInfo(
    Guid SupportTicketDetailId,
    Guid OrderItemId,
    Guid? DriveId,
    int Quantity,
    string? Note);
