using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.UpdateInstockProduct;

internal sealed class UpdateInstockProductCommandHandler : ICommandHandler<UpdateInstockProductCommand>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockProductCommandHandler(
        IInstockProductRepository productRepository,
        IInstockProductVariantRepository variantRepository,
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _variantRepository = variantRepository;
        _priceDetailRepository = priceDetailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInstockProductCommand request, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(InstockProductError.NotFound(request.Id));
        }

        if (request.Slug is not null)
        {
            var existingProduct = await _productRepository.GetBySlugAsync(request.Slug, cancellationToken);
            if (existingProduct is not null && existingProduct.Id != productId)
            {
                return Result.Failure(InstockProductError.DuplicateSlug(request.Slug));
            }
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var updateResult = product.PartialUpdate(
                request.Slug,
                request.Name,
                request.TotalPieceCount,
                request.DifficultLevel,
                request.EstimatedBuildTime,
                request.ThumbnailUrl,
                request.PreviewAsset,
                request.TopicId,
                request.AssemblyMethodId,
                request.MaterialId,
                request.CapabilityIds,
                request.Description,
                request.IsActive);

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            // Update capabilities if provided
            if (request.CapabilityIds != null)
            {
                product.SetCapabilities(request.CapabilityIds);
            }

            // If IsActive is set to false, deactivate all variants of this product
            if (request.IsActive.HasValue && request.IsActive.Value == false)
            {
                var variants = await _variantRepository.GetAllByProductIdAsync(productId, cancellationToken);
                foreach (var variant in variants)
                {
                    if (variant.IsActive)
                    {
                        var variantUpdateResult = variant.PartialUpdate(isActive: false);
                        if (variantUpdateResult.IsFailure)
                        {
                            return variantUpdateResult;
                        }

                        // Also deactivate all price details for this variant
                        var priceDetails = await _priceDetailRepository.GetAllByProductVariantIdAsync(variant.Id, cancellationToken);
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

                        _variantRepository.Update(variant);
                    }
                }
            }

            _productRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
    }
}
