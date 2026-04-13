using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.GetFormulaValueValidationById;

internal sealed class GetFormulaValueValidationByIdQueryHandler
    : IQueryHandler<GetFormulaValueValidationByIdQuery, object>
{
    private readonly IFormulaValueValidationRepository _repository;

    public GetFormulaValueValidationByIdQueryHandler(IFormulaValueValidationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<object>> Handle(
        GetFormulaValueValidationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var validationId = FormulaValueValidationId.From(request.Id);
        var validation = await _repository.GetByIdAsync(validationId, cancellationToken);

        if (validation is null)
            return Result.Failure<object>(FormulaValueValidationError.NotFound(request.Id));

        var response = new GetFormulaValueValidationDetailedResponseDto(
            validation.Id.Value,
            validation.FormulaId.Value,
            validation.MinValue,
            validation.MaxValue,
            validation.Output,
            validation.UpdatedAt);

        return Result.Success<object>(response);
    }
}
