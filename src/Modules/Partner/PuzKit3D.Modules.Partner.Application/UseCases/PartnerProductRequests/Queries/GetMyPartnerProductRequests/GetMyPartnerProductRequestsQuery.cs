using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetMyPartnerProductRequests;

public sealed record GetMyPartnerProductRequestsQuery(
    Guid CustomerId,
    int? Status = null,
    DateTime? CreatedAtFrom = null,
    DateTime? CreatedAtTo = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<object>>;
