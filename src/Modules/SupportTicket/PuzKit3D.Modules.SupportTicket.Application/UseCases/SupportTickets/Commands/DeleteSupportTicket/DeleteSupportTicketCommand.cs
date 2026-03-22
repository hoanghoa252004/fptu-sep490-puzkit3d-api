using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.DeleteSupportTicket;

public sealed record DeleteSupportTicketCommand(Guid SupportTicketId) : ICommand;
