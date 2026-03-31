using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetAllPartnerProductQuotations;

public sealed record GetAllPartnerProductQuotationsQuery(
    DateTime? CreatedAtFrom,
    DateTime? CreatedAtTo,
    bool Ascending,
    int PageNumber,
    int PageSize) : IQuery<PagedResult<GetAllPartnerProductQuotationsResponseDto>>;
