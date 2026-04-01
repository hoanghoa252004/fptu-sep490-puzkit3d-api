using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetManagerPartnerProductRequests;

public sealed record GetManagerPartnerProductRequestsQuery(
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<object>>;
