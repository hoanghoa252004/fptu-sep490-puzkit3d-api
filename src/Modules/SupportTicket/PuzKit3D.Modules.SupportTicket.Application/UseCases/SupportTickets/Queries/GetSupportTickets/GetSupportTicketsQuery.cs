using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries.GetSupportTickets;

public sealed record GetSupportTicketsQuery(
    int PageNumber,
    int PageSize,
    string? Status = null) : IQuery<PagedResult<SupportTicketDto>>;

