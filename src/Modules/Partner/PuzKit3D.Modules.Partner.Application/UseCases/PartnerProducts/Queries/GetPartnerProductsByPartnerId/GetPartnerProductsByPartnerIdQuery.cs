using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductsByPartnerId;

public sealed record GetPartnerProductsByPartnerIdQuery(
    Guid PartnerId,
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool Ascending = false) : IQuery<PagedResult<object>>;