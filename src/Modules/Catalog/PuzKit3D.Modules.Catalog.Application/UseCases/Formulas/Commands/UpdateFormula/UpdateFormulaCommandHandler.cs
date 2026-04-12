using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.UpdateFormula;

internal sealed class UpdateFormulaCommandHandler
    : ICommandTHandler<UpdateFormulaCommand, Guid>
{
    private readonly IFormulaRepository _formulaRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateFormulaCommandHandler(
        IFormulaRepository formulaRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _formulaRepository = formulaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        UpdateFormulaCommand request,
        CancellationToken cancellationToken)
    {
        var formulaId = FormulaId.From(request.Id);
        var formula = await _formulaRepository.GetByIdAsync(formulaId, cancellationToken);

        if (formula is null)
            return Result.Failure<Guid>(FormulaError.NotFound(request.Id));

        // Check if new code already exists (if changing code)
        if (!string.IsNullOrWhiteSpace(request.Code) && request.Code != formula.Code)
        {
            var allFormulas = await _formulaRepository.GetAllAsync(cancellationToken);
            var existingFormula = allFormulas.FirstOrDefault(f => f.Code == request.Code);

            if (existingFormula is not null)
                return Result.Failure<Guid>(FormulaError.CodeAlreadyExists(request.Code));
        }

        // Update formula
        formula.Update(request.Code, request.Expression, request.Description);

        _formulaRepository.Update(formula);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(formula.Id.Value);
    }
}
