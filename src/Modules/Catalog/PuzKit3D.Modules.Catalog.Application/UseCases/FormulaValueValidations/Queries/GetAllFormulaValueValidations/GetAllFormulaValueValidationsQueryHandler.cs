using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.GetAllFormulaValueValidations;

internal sealed class GetAllFormulaValueValidationsQueryHandler
    : IQueryHandler<GetAllFormulaValueValidationsQuery, List<object>>
{
    private readonly IFormulaValueValidationRepository _repository;

    public GetAllFormulaValueValidationsQueryHandler(IFormulaValueValidationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<List<object>>> Handle(
        GetAllFormulaValueValidationsQuery request,
        CancellationToken cancellationToken)
    {
        var allValidations = await _repository.GetAllAsync(cancellationToken);

        // Build response DTOs
        var validationDtos = allValidations
            .Select(v => (object)new GetFormulaValueValidationDetailedResponseDto(
                v.Id.Value,
                v.FormulaId.Value,
                v.MinValue,
                v.MaxValue,
                v.Output,
                v.UpdatedAt))
            .ToList();

        return Result.Success(validationDtos);
    }
}

