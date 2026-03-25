using MediatR;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries;

public sealed record SupportTicketDto(
    Guid Id,
    string Code,
    Guid UserId,
    Guid OrderId,
    string Type,
    string Status,
    string Reason,
    string Proof,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<SupportTicketDetailDto> Details);
