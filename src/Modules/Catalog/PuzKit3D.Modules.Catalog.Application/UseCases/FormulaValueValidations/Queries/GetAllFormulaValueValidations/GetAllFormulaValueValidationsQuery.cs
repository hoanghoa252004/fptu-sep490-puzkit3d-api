using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.GetAllFormulaValueValidations;

public sealed record GetAllFormulaValueValidationsQuery(
    int PageNumber,
    int PageSize,
    Guid? FormulaId = null,
    bool Ascending = true) : IQuery<PagedResult<object>>;
