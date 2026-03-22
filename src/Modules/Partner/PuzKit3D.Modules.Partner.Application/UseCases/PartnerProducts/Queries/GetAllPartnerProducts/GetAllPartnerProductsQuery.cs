using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetAllPartnerProducts;

public sealed record GetAllPartnerProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    Guid? PartnerId = null) : IQuery<PagedResult<object>>;
