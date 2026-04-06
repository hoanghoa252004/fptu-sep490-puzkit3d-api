using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetAllPartnerProductOrders;

public sealed record GetAllPartnerProductOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Status = null,
    DateTime? CreatedAtFrom = null,
    DateTime? CreatedAtTo = null,
    bool Ascending = false) : IQuery<PagedResult<object>>;
