using MediatR;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.DeleteSupportTicket;

public sealed record DeleteSupportTicketCommand(Guid SupportTicketId) : IRequest<DeleteSupportTicketResponse>;

public sealed record DeleteSupportTicketResponse(
    bool Success,
    string Message);
