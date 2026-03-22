using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Commands.UpdateInventory;

internal sealed class UpdateInventoryCommandHandler : ICommandHandler<UpdateInventoryCommand>
{
    private readonly IInstockInventoryRepository _inventoryRepository;
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInventoryCommandHandler(
        IInstockInventoryRepository inventoryRepository,
        IInstockProductRepository productRepository,
        IInstockProductVariantRepository variantRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
        _variantRepository = variantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        // Validate product exists
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        
        if (product is null)
        {
            return Result.Failure(InstockProductError.NotFound(request.ProductId));
        }

        // Validate variant exists and belongs to product
        var variantId = InstockProductVariantId.From(request.VariantId);
        var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);
        
        if (variant is null)
        {
            return Result.Failure(InstockProductVariantError.NotFound(request.VariantId));
        }

        if (variant.InstockProductId.Value != request.ProductId)
        {
            return Result.Failure(
                InstockProductVariantError.VariantDoesNotBelongToProduct(request.VariantId, request.ProductId));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            // Get inventory - must exist
            var inventory = await _inventoryRepository.GetByVariantIdAsync(request.VariantId, cancellationToken);

            if (inventory is null)
            {
                return Result.Failure(InstockInventoryError.NotFound(request.VariantId));
            }

            // Update existing inventory
            var updateResult = inventory.SetStock(request.Quantity);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            _inventoryRepository.Update(inventory);

            return Result.Success();
        }, cancellationToken);
    }
}
