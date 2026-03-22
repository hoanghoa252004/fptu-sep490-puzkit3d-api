using MediatR;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.UpdateSupportTicketStatus;

public sealed record UpdateSupportTicketStatusCommand(
    Guid SupportTicketId,
    SupportTicketStatus NewStatus) : ICommand;