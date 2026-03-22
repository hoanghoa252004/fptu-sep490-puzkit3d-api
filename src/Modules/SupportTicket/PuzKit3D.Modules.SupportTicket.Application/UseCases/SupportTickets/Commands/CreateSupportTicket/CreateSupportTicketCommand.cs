using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.CreateSupportTicket;

public sealed record CreateSupportTicketCommand(
    Guid UserId,
    Guid OrderId,
    SupportTicketType Type,
    string Reason,
    string Proof) : ICommandT<Guid>;
