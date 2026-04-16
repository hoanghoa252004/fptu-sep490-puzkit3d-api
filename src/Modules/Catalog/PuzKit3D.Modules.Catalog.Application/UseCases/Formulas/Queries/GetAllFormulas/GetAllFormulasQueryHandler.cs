using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetAllFormulas;

internal sealed class GetAllFormulasQueryHandler
    : IQueryHandler<GetAllFormulasQuery, List<GetFormulaDetailedResponseDto>>
{
    private readonly IFormulaRepository _formulaRepository;

    public GetAllFormulasQueryHandler(IFormulaRepository formulaRepository)
    {
        _formulaRepository = formulaRepository;
    }

    public async Task<ResultT<List<GetFormulaDetailedResponseDto>>> Handle(
        GetAllFormulasQuery request,
        CancellationToken cancellationToken)
    {
        // Get all formulas
        var allFormulas = await _formulaRepository.GetAllAsync(cancellationToken);

        // Build response DTOs
        var formulaDtos = allFormulas
            .Select(f => 
            {
                // Build FormulaValueValidations list if IsNeedValidation is true
                List<FormulaValueValidationDto>? validationDtos = null;
                if (f.IsNeedValidation && f.FormulaValueValidations.Any())
                {
                    validationDtos = f.FormulaValueValidations
                        .Select(v => new FormulaValueValidationDto(
                            v.Id.Value,
                            v.MinValue,
                            v.MaxValue,
                            v.Output))
                        .OrderBy(v => v.MinValue)
                        .ToList();
                }

                return new GetFormulaDetailedResponseDto(
                    Id: f.Id.Value,
                    Code: f.Code.ToString(),
                    Expression: f.Expression,
                    Description: f.Description,
                    IsNeedValidation: f.IsNeedValidation,
                    UpdatedAt: f.UpdatedAt,
                    FormulaValueValidations: validationDtos);
            })
            .ToList();

        return Result.Success(formulaDtos);
    }
}


