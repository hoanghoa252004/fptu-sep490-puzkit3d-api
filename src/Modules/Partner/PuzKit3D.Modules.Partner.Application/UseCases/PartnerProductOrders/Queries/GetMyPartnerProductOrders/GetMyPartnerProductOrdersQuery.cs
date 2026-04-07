using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetMyPartnerProductOrders;

public sealed record GetMyPartnerProductOrdersQuery(
    Guid CustomerId,
    int PageNumber = 1,
    int PageSize = 10,
    string? Status = null) : IQuery<PagedResult<object>>;