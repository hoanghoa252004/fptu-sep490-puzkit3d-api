using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries.GetSupportTicketByOrderId;

public sealed record GetSupportTicketByOrderIdQuery(Guid OrderId) : IQuery<SupportTicketDto>;
