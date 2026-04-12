using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetAllFormulas;

internal sealed class GetAllFormulasQueryHandler
    : IQueryHandler<GetAllFormulasQuery, PagedResult<object>>
{
    private readonly IFormulaRepository _formulaRepository;

    public GetAllFormulasQueryHandler(IFormulaRepository formulaRepository)
    {
        _formulaRepository = formulaRepository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllFormulasQuery request,
        CancellationToken cancellationToken)
    {
        // Get all formulas
        var allFormulas = await _formulaRepository.GetAllAsync(cancellationToken);

        // Filter by search term
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            allFormulas = allFormulas.Where(f => 
                f.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                f.Expression.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (f.Description != null && f.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        // Apply sorting
        allFormulas = request.Ascending 
            ? allFormulas.OrderBy(f => f.Code).ToList()
            : allFormulas.OrderByDescending(f => f.Code).ToList();

        // Apply pagination
        var totalCount = allFormulas.Count();
        var formulas = allFormulas
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        var formulaDtos = formulas.Select(f => (object)new GetFormulaDetailedResponseDto(
            f.Id.Value,
            f.Code,
            f.Expression,
            f.Description,
            f.UpdatedAt)).ToList();

        var pagedResult = new PagedResult<object>(
            formulaDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
