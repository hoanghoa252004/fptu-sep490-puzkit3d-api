using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.DeleteFormulaValueValidation;

internal sealed class DeleteFormulaValueValidationCommandHandler
    : ICommandHandler<DeleteFormulaValueValidationCommand>
{
    private readonly IFormulaValueValidationRepository _repository;
    private readonly IFormulaRepository _formulaRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteFormulaValueValidationCommandHandler(
        IFormulaValueValidationRepository repository,
        IFormulaRepository formulaRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _formulaRepository = formulaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteFormulaValueValidationCommand request,
        CancellationToken cancellationToken)
    {
        var validationId = FormulaValueValidationId.From(request.Id);
        var validation = await _repository.GetByIdAsync(validationId, cancellationToken);

        if (validation is null)
            return Result.Failure(FormulaValueValidationError.NotFound(request.Id));

        _repository.Delete(validation);

        // Check if formula has any remaining validations
        var formula = await _formulaRepository.GetByIdAsync(validation.FormulaId, cancellationToken);
        if (formula is not null)
        {
            var remainingValidations = formula.FormulaValueValidations.Where(v => v.Id != validationId).ToList();
            if (!remainingValidations.Any() && formula.IsNeedValidation)
            {
                formula.SetNeedValidation(false);
                _formulaRepository.Update(formula);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


