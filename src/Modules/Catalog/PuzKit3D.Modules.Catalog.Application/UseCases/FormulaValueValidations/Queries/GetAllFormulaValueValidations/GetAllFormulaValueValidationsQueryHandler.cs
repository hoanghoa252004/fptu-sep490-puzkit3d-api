using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.GetAllFormulaValueValidations;

internal sealed class GetAllFormulaValueValidationsQueryHandler
    : IQueryHandler<GetAllFormulaValueValidationsQuery, PagedResult<object>>
{
    private readonly IFormulaValueValidationRepository _repository;

    public GetAllFormulaValueValidationsQueryHandler(IFormulaValueValidationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllFormulaValueValidationsQuery request,
        CancellationToken cancellationToken)
    {
        var allValidations = await _repository.GetAllAsync(cancellationToken);

        // Filter by FormulaId if provided
        if (request.FormulaId.HasValue)
        {
            var formulaId = FormulaId.From(request.FormulaId.Value);
            allValidations = allValidations.Where(fvv => fvv.FormulaId == formulaId).ToList();
        }

        // Apply sorting
        allValidations = request.Ascending
            ? allValidations.OrderBy(fvv => fvv.MinValue).ToList()
            : allValidations.OrderByDescending(fvv => fvv.MinValue).ToList();

        // Apply pagination
        var totalCount = allValidations.Count();
        var validations = allValidations
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        var validationDtos = validations.Select(v => (object)new GetFormulaValueValidationDetailedResponseDto(
            v.Id.Value,
            v.FormulaId.Value,
            v.MinValue,
            v.MaxValue,
            v.Output,
            v.UpdatedAt)).ToList();

        var pagedResult = new PagedResult<object>(
            validationDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
