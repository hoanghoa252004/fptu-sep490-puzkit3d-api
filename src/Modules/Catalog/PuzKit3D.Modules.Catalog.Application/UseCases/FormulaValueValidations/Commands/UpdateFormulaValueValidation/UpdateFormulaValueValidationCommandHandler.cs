using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.UpdateFormulaValueValidation;

internal sealed class UpdateFormulaValueValidationCommandHandler
    : ICommandTHandler<UpdateFormulaValueValidationCommand, Guid>
{
    private readonly IFormulaValueValidationRepository _repository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateFormulaValueValidationCommandHandler(
        IFormulaValueValidationRepository repository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        UpdateFormulaValueValidationCommand request,
        CancellationToken cancellationToken)
    {
        var validationId = FormulaValueValidationId.From(request.Id);
        var validation = await _repository.GetByIdAsync(validationId, cancellationToken);

        if (validation is null)
            return Result.Failure<Guid>(FormulaValueValidationError.NotFound(request.Id));

        // Validate range if both values are provided
        var minValue = request.MinValue ?? validation.MinValue;
        var maxValue = request.MaxValue ?? validation.MaxValue;

        if (minValue > maxValue)
            return Result.Failure<Guid>(FormulaValueValidationError.InvalidRange());

        // Update validation
        validation.Update(request.MinValue, request.MaxValue, request.Output);

        _repository.Update(validation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(validation.Id.Value);
    }
}
