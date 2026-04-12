using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetFormulaById;

internal sealed class GetFormulaByIdQueryHandler
    : IQueryHandler<GetFormulaByIdQuery, object>
{
    private readonly IFormulaRepository _formulaRepository;

    public GetFormulaByIdQueryHandler(IFormulaRepository formulaRepository)
    {
        _formulaRepository = formulaRepository;
    }

    public async Task<ResultT<object>> Handle(
        GetFormulaByIdQuery request,
        CancellationToken cancellationToken)
    {
        var formulaId = FormulaId.From(request.Id);
        var formula = await _formulaRepository.GetByIdAsync(formulaId, cancellationToken);

        if (formula is null)
            return Result.Failure<object>(FormulaError.NotFound(request.Id));

        var response = new GetFormulaDetailedResponseDto(
            formula.Id.Value,
            formula.Code,
            formula.Expression,
            formula.Description,
            formula.UpdatedAt);

        return Result.Success<object>(response);
    }
}
