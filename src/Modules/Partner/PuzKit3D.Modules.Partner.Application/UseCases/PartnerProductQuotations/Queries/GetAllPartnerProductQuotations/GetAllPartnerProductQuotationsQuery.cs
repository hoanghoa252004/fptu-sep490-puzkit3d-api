using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetAllPartnerProductQuotations;

public sealed record GetAllPartnerProductQuotationsQuery(
    int? Status,
    bool Ascending,
    string? SearchTerm,
    int PageNumber,
    int PageSize) : IQuery<PagedResult<GetAllPartnerProductQuotationsResponseDto>>;
