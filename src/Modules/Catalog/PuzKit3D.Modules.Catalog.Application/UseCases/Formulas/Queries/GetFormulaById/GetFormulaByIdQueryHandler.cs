using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetFormulaById;

internal sealed class GetFormulaByIdQueryHandler
    : IQueryHandler<GetFormulaByIdQuery, GetFormulaDetailedResponseDto>
{
    private readonly IFormulaRepository _formulaRepository;

    public GetFormulaByIdQueryHandler(IFormulaRepository formulaRepository)
    {
        _formulaRepository = formulaRepository;
    }

    public async Task<ResultT<GetFormulaDetailedResponseDto>> Handle(
        GetFormulaByIdQuery request,
        CancellationToken cancellationToken)
    {
        var formulaId = FormulaId.From(request.Id);
        var formula = await _formulaRepository.GetByIdAsync(formulaId, cancellationToken);

        if (formula is null)
            return Result.Failure<GetFormulaDetailedResponseDto>(FormulaError.NotFound(request.Id));

        // Build FormulaValueValidations list if IsNeedValidation is true
        List<FormulaValueValidationDto>? validationDtos = null;
        if (formula.IsNeedValidation && formula.FormulaValueValidations.Any())
        {
            validationDtos = formula.FormulaValueValidations
                .Select(v => new FormulaValueValidationDto(
                    v.Id.Value,
                    v.MinValue,
                    v.MaxValue,
                    v.Output))
                .OrderBy(v => v.MinValue)
                .ToList();
        }

        var response = new GetFormulaDetailedResponseDto(
            Id: formula.Id.Value,
            Code: formula.Code.ToString(),
            Expression: formula.Expression,
            Description: formula.Description,
            IsNeedValidation: formula.IsNeedValidation,
            UpdatedAt: formula.UpdatedAt,
            FormulaValueValidations: validationDtos);

        return Result.Success(response);
    }
}
