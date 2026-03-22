using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Commands.DeleteInventory;

internal sealed class DeleteInventoryCommandHandler : ICommandHandler<DeleteInventoryCommand>
{
    private readonly IInstockInventoryRepository _inventoryRepository;
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public DeleteInventoryCommandHandler(
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

    public async Task<Result> Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
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

        // Get inventory
        var inventory = await _inventoryRepository.GetByVariantIdAsync(request.VariantId, cancellationToken);

        // If no inventory exists, just return success (already deleted/zero)
        if (inventory is null)
        {
            return Result.Success();
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            // Delete inventory - publish event
            inventory.Delete();
            _inventoryRepository.Delete(inventory);

            return Result.Success();
        }, cancellationToken);
    }
}
