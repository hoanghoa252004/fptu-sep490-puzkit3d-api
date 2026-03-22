using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries.GetSupportTicketById;

public sealed record GetSupportTicketByIdQuery(Guid SupportTicketId) : IQuery<SupportTicketDto>;
