using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetAllPartners;

public sealed record GetAllPartnersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null) : IQuery<PagedResult<object>>;
