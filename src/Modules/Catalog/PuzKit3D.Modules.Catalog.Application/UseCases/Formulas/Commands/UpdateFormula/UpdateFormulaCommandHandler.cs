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

        // Update formula - only Expression and Description can be updated
        formula.Update(
            request.Expression ?? formula.Expression,
            request.Description ?? formula.Description);

        _formulaRepository.Update(formula);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(formula.Id.Value);
    }
}
