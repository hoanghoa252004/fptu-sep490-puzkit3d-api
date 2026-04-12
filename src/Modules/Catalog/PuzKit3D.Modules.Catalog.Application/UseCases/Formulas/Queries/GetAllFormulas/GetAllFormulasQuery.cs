using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetAllFormulas;

public sealed record GetAllFormulasQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm = null,
    bool Ascending = true) : IQuery<PagedResult<object>>;
