using MediatR;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries;

public sealed record SupportTicketDto(
    Guid Id,
    Guid UserId,
    Guid OrderId,
    SupportTicketType Type,
    SupportTicketStatus Status,
    string Reason,
    string Proof,
    DateTime CreatedAt,
    DateTime UpdatedAt);
