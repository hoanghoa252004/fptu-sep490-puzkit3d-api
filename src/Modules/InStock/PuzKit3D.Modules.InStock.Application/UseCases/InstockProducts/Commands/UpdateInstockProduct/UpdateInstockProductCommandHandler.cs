using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.UpdateInstockProduct;

internal sealed class UpdateInstockProductCommandHandler : ICommandHandler<UpdateInstockProductCommand>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInstockProductDriveRepository _productDriveRepository;
    private readonly ICapabilityReplicaRepository _capabilityReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;
    private readonly IDriveReplicaRepository _driveReplicaRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockProductCommandHandler(
        IInstockProductRepository productRepository,
        IInstockProductVariantRepository variantRepository,
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInstockProductDriveRepository productDriveRepository,
        ICapabilityReplicaRepository capabilityReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository,
        IDriveReplicaRepository driveReplicaRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _variantRepository = variantRepository;
        _priceDetailRepository = priceDetailRepository;
        _productDriveRepository = productDriveRepository;
        _capabilityReplicaRepository = capabilityReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
        _driveReplicaRepository = driveReplicaRepository;
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

        // Validate capabilities exist in replica if provided
        if (request.CapabilityIds != null && request.CapabilityIds.Count > 0)
        {
            foreach (var capabilityId in request.CapabilityIds)
            {
                var capabilityExists = await _capabilityReplicaRepository.ExistsByIdAsync(capabilityId, cancellationToken);
                if (!capabilityExists)
                {
                    return Result.Failure(InstockProductError.InvalidCapability(capabilityId));
                }
            }
        }

        // Validate assembly methods exist in replica if provided
        if (request.AssemblyMethodIds != null && request.AssemblyMethodIds.Count > 0)
        {
            foreach (var assemblyMethodId in request.AssemblyMethodIds)
            {
                var assemblyMethodExists = await _assemblyMethodReplicaRepository.ExistsByIdAsync(assemblyMethodId, cancellationToken);
                if (!assemblyMethodExists)
                {
                    return Result.Failure(InstockProductError.InvalidAssemblyMethod());
                }
            }
        }

        // Validate drive details if provided
        if (request.Drives != null && request.Drives.Count > 0)
        {
            foreach (var driveDetail in request.Drives)
            {
                var driveExists = await _driveReplicaRepository.ExistsByIdAsync(driveDetail.DriveId, cancellationToken);
                if (!driveExists)
                {
                    return Result.Failure(InstockProductError.InvalidDrive(driveDetail.DriveId));
                }

                if (driveDetail.Quantity <= 0)
                {
                    return Result.Failure(InstockProductError.InvalidQuantity());
                }
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
                request.MaterialId,
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

            // Update assembly methods if provided
            if (request.AssemblyMethodIds != null)
            {
                product.SetAssemblyMethods(request.AssemblyMethodIds);
            }

            // Update drives if provided
            if (request.Drives != null)
            {
                var drivesList = request.Drives.Select(d => (d.DriveId, d.Quantity)).ToList();
                product.SetDrives(drivesList);
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
