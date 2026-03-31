using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetStaffPartnerProductRequests;

public sealed record GetStaffPartnerProductRequestsQuery(
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<object>>;
