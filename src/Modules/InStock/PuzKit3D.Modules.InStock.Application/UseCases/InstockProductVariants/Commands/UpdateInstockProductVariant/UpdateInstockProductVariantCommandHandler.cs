using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.UpdateInstockProductVariant;

internal sealed class UpdateInstockProductVariantCommandHandler : ICommandHandler<UpdateInstockProductVariantCommand>
{
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockProductVariantCommandHandler(
        IInstockProductVariantRepository variantRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _variantRepository = variantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInstockProductVariantCommand request, CancellationToken cancellationToken)
    {
        var variantId = InstockProductVariantId.From(request.VariantId);
        var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);

        if (variant is null)
        {
            return Result.Failure(InstockProductVariantError.NotFound(request.VariantId));
        }

        if (request.Sku is not null)
        {
            var existingBySku = await _variantRepository.GetBySkuAsync(request.Sku, cancellationToken);
            if (existingBySku is not null && existingBySku.Id != variant.Id)
            {
                return Result.Failure(InstockProductVariantError.DuplicateSku(request.Sku));
            }
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var updateResult = variant.PartialUpdate(
                request.Sku,
                request.Color,
                request.AssembledLengthMm,
                request.AssembledWidthMm,
                request.AssembledHeightMm);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            _variantRepository.Update(variant);

            return Result.Success();
        }, cancellationToken);
    }
}
