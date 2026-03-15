using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Commands.CreateInventory;

internal sealed class CreateInventoryCommandHandler : ICommandHandler<CreateInventoryCommand>
{
    private readonly IInstockInventoryRepository _inventoryRepository;
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreateInventoryCommandHandler(
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

    public async Task<Result> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
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

        // Check inventory doesn't already exist
        var existingInventory = await _inventoryRepository.GetByVariantIdAsync(request.VariantId, cancellationToken);
        if (existingInventory is not null)
        {
            return Result.Failure(InstockInventoryError.AlreadyExists(request.VariantId));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            // Create new inventory
            var createResult = InstockInventory.Create(variantId, request.Quantity);
            
            if (createResult.IsFailure)
            {
                return Result.Failure(createResult.Error);
            }

            _inventoryRepository.Add(createResult.Value);

            return Result.Success();
        }, cancellationToken);
    }
}
