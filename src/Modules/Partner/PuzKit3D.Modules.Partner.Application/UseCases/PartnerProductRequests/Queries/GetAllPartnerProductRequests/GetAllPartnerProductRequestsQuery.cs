using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetAllPartnerProductRequests;

public sealed record GetAllPartnerProductRequestsQuery(
    int? Status = null,
    string? SearchTerm = null,
    bool Ascending = false,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<object>>;
