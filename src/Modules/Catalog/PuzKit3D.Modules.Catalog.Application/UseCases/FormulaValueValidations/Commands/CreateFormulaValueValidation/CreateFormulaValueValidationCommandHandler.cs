using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.CreateFormulaValueValidation;

internal sealed class CreateFormulaValueValidationCommandHandler
    : ICommandTHandler<CreateFormulaValueValidationCommand, Guid>
{
    private readonly IFormulaValueValidationRepository _repository;
    private readonly IFormulaRepository _formulaRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateFormulaValueValidationCommandHandler(
        IFormulaValueValidationRepository repository,
        IFormulaRepository formulaRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _formulaRepository = formulaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        CreateFormulaValueValidationCommand request,
        CancellationToken cancellationToken)
    {
        // Validate formula exists
        var formulaId = FormulaId.From(request.FormulaId);
        var formula = await _formulaRepository.GetByIdAsync(formulaId, cancellationToken);

        if (formula is null)
            return Result.Failure<Guid>(FormulaError.NotFound(request.FormulaId));

        // Validate input
        if (string.IsNullOrWhiteSpace(request.Output))
            return Result.Failure<Guid>(FormulaValueValidationError.InvalidOutput());

        if (request.MinValue > request.MaxValue)
            return Result.Failure<Guid>(FormulaValueValidationError.InvalidRange());

        // Create validation
        var validation = FormulaValueValidation.Create(
            formulaId,
            request.MinValue,
            request.MaxValue,
            request.Output);

        _repository.Add(validation.Value);

        // If formula IsNeedValidation is false, update it to true
        if (!formula.IsNeedValidation)
        {
            formula.SetNeedValidation(true);
            _formulaRepository.Update(formula);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(validation.Value.Id.Value);
    }
}
