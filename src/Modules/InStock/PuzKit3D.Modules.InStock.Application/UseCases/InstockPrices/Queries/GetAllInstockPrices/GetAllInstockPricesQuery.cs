using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Queries.GetAllInstockPrices;

public sealed record GetAllInstockPricesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PagedResult<object>>;
