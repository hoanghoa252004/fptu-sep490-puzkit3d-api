using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Queries.GetInstockInventoryByVariantId;

internal sealed class GetInstockInventoryByVariantIdQueryHandler 
    : IQueryHandler<GetInstockInventoryByVariantIdQuery, GetInstockInventoryByVariantIdResponseDto>
{
    private readonly IInstockInventoryRepository _inventoryRepository;
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;

    public GetInstockInventoryByVariantIdQueryHandler(
        IInstockInventoryRepository inventoryRepository,
        IInstockProductRepository productRepository,
        IInstockProductVariantRepository variantRepository)
    {
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
        _variantRepository = variantRepository;
    }

    public async Task<ResultT<GetInstockInventoryByVariantIdResponseDto>> Handle(
        GetInstockInventoryByVariantIdQuery request, 
        CancellationToken cancellationToken)
    {
        // Validate product exists
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        
        if (product is null)
        {
            return Result.Failure<GetInstockInventoryByVariantIdResponseDto>(
                InstockProductError.NotFound(request.ProductId));
        }

        // Validate variant exists and belongs to product
        var variantId = InstockProductVariantId.From(request.VariantId);
        var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);
        
        if (variant is null)
        {
            return Result.Failure<GetInstockInventoryByVariantIdResponseDto>(
                InstockProductVariantError.NotFound(request.VariantId));
        }

        if (variant.InstockProductId.Value != request.ProductId)
        {
            return Result.Failure<GetInstockInventoryByVariantIdResponseDto>(
                InstockProductVariantError.VariantDoesNotBelongToProduct(request.VariantId, request.ProductId));
        }

        // Get inventory
        var inventory = await _inventoryRepository.GetByVariantIdAsync(request.VariantId, cancellationToken);

        if (inventory is null)
        {
            return Result.Failure<GetInstockInventoryByVariantIdResponseDto>(
                InstockInventoryError.NotFoundByVariantId(request.VariantId));
        }

        var response = new GetInstockInventoryByVariantIdResponseDto(
            inventory.Id.Value,
            inventory.InstockProductVariantId.Value,
            inventory.TotalQuantity,
            inventory.CreatedAt,
            inventory.UpdatedAt);

        return Result.Success(response);
    }
}
