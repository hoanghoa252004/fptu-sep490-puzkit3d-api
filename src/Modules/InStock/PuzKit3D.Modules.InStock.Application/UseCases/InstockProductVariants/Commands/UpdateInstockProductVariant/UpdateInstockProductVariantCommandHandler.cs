using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.UpdateInstockProductVariant;

internal sealed class UpdateInstockProductVariantCommandHandler : ICommandHandler<UpdateInstockProductVariantCommand>
{
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockProductVariantCommandHandler(
        IInstockProductVariantRepository variantRepository,
        IInstockProductRepository productRepository,
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _variantRepository = variantRepository;
        _productRepository = productRepository;
        _priceDetailRepository = priceDetailRepository;
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

        // If trying to activate the variant, check if the associated product is also active
        if (request.IsActive.HasValue && request.IsActive.Value == true)
        {
            var product = await _productRepository.GetByIdAsync(variant.InstockProductId, cancellationToken);
            if (product is null || !product.IsActive)
            {
                return Result.Failure(InstockProductVariantError.CannotActivateVariantWithInactiveProduct());
            }
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var updateResult = variant.PartialUpdate(
                null, // SKU cannot be updated
                request.Color,
                request.AssembledLengthMm,
                request.AssembledWidthMm,
                request.AssembledHeightMm,
                request.IsActive);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            // If IsActive is set to false, deactivate all price details for this variant
            if (request.IsActive.HasValue && request.IsActive.Value == false)
            {
                var priceDetails = await _priceDetailRepository.GetAllByProductVariantIdAsync(variantId, cancellationToken);
                foreach (var priceDetail in priceDetails)
                {
                    if (priceDetail.IsActive)
                    {
                        var priceDetailUpdateResult = priceDetail.PartialUpdate(isActive: false);
                        if (priceDetailUpdateResult.IsFailure)
                        {
                            return priceDetailUpdateResult;
                        }

                        _priceDetailRepository.Update(priceDetail);
                    }
                }
            }

            _variantRepository.Update(variant);

            return Result.Success();
        }, cancellationToken);
    }
}
