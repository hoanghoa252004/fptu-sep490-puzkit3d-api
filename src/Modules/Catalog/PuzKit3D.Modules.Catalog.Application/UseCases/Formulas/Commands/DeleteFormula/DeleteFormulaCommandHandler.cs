using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.DeleteFormula;

internal sealed class DeleteFormulaCommandHandler
    : ICommandHandler<DeleteFormulaCommand>
{
    private readonly IFormulaRepository _formulaRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteFormulaCommandHandler(
        IFormulaRepository formulaRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _formulaRepository = formulaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteFormulaCommand request,
        CancellationToken cancellationToken)
    {
        var formulaId = FormulaId.From(request.Id);
        var formula = await _formulaRepository.GetByIdAsync(formulaId, cancellationToken);

        if (formula is null)
            return Result.Failure(FormulaError.NotFound(request.Id));

        _formulaRepository.Delete(formula);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
