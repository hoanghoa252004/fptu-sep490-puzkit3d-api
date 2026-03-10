using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.DeleteInstockProductVariant;

internal sealed class DeleteInstockProductVariantCommandHandler : ICommandHandler<DeleteInstockProductVariantCommand>
{
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public DeleteInstockProductVariantCommandHandler(
        IInstockProductVariantRepository variantRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _variantRepository = variantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteInstockProductVariantCommand request, CancellationToken cancellationToken)
    {
        var variantId = InstockProductVariantId.From(request.VariantId);
        var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);

        if (variant is null)
        {
            return Result.Failure(InstockProductVariantError.NotFound(request.VariantId));
        }

        if (!variant.IsActive)
        {
            return Result.Failure(InstockProductVariantError.AlreadyInactive(request.VariantId));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            variant.Deactivate();
            _variantRepository.Update(variant);

            return Result.Success();
        }, cancellationToken);
    }
}
