using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CreateFormula;

internal sealed class CreateFormulaCommandHandler
    : ICommandTHandler<CreateFormulaCommand, Guid>
{
    private readonly IFormulaRepository _formulaRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateFormulaCommandHandler(
        IFormulaRepository formulaRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _formulaRepository = formulaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        CreateFormulaCommand request,
        CancellationToken cancellationToken)
    {
        // Check if code already exists
        var allFormulas = await _formulaRepository.GetAllAsync(cancellationToken);
        var existingFormula = allFormulas.FirstOrDefault(f => f.Code == request.Code);

        if (existingFormula is not null)
            return Result.Failure<Guid>(FormulaError.CodeAlreadyExists(request.Code));

        // Create formula (validation happens in domain)
        var createResult = Formula.Create(
            request.Code,
            request.Expression,
            request.Description);

        if (createResult.IsFailure)
            return Result.Failure<Guid>(createResult.Error);

        var formula = createResult.Value;

        _formulaRepository.Add(formula);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(formula.Id.Value);
    }



}
