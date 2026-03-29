using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Queries.GetCart;

internal sealed class GetInStockCartQueryHandler : IQueryHandler<GetInStockCartQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;
    private readonly IMediaAssetService _assetUrlService;

    public GetInStockCartQueryHandler(
        ICartRepository cartRepository,
        ICartQueryRepository queryRepository,
        IMediaAssetService assetUrlService)
    {
        _cartRepository = cartRepository;
        _queryRepository = queryRepository;
        _assetUrlService = assetUrlService;
    }

    public async Task<ResultT<CartDto>> Handle(GetInStockCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            request.UserId,
            "INSTOCK",
            cancellationToken);

        if (cart == null)
            return Result.Failure<CartDto>(CartError.CartNotFound());

        var itemIds = cart.Items.Select(i => i.ItemId).ToList();
        
        // Get InStock variant details
        var variants = await _queryRepository.GetInStockProductVariantsByIdsAsync(itemIds, cancellationToken);
        
        // Get InStock product details (for thumbnail)
        var productIds = variants.Values.Select(v => v.InStockProductId).Distinct().ToList();
        var products = await _queryRepository.GetInStockProductsByIdsAsync(productIds, cancellationToken);
        
        // Get inventory for all variants
        var inventoriesMap = await _queryRepository.GetInStockInventoriesByVariantIdsAsync(itemIds, cancellationToken);
        
        var productDetailsMap = variants.ToDictionary(
            kvp => kvp.Key,
            kvp => 
            {
                var product = products.TryGetValue(kvp.Value.InStockProductId, out var p) ? p : null;
                return new ProductDetailsDto(
                    kvp.Value.InStockProductId,
                    product?.Name ?? string.Empty,
                    product?.Slug ?? string.Empty,
                    $"{kvp.Value.Color} - {kvp.Value.AssembledLengthMm}x{kvp.Value.AssembledWidthMm}x{kvp.Value.AssembledHeightMm}mm",
                    kvp.Value.Sku,
                    kvp.Value.Color,
                    kvp.Value.AssembledLengthMm,
                    kvp.Value.AssembledWidthMm,
                    kvp.Value.AssembledHeightMm,
                    product != null ? _assetUrlService.BuildAssetUrl(product.ThumbnailUrl) : null,
                    kvp.Value.IsActive,
                    null,
                    null);
            });

        // Get price details for all cart items
        var priceDetailIds = cart.Items
            .Where(i => i.InStockProductPriceDetailId.HasValue)
            .Select(i => i.InStockProductPriceDetailId!.Value)
            .Distinct()
            .ToList();

        var priceDetailsMap = new Dictionary<Guid, decimal>();
        foreach (var priceDetailId in priceDetailIds)
        {
            var priceDetail = await _queryRepository.GetInStockPriceDetailByIdAsync(priceDetailId, cancellationToken);
            if (priceDetail != null)
            {
                priceDetailsMap[priceDetailId] = priceDetail.UnitPrice;
            }
        }

        // Build cart items with validation, sorted by CreatedAt (newest first)
        var cartItems = new List<CartItemDto>();
        foreach (var item in cart.Items.OrderByDescending(i => i.CreatedAt))
        {
            decimal? unitPrice = null;
            decimal? totalPrice = null;
            bool isVariantActive = true;
            bool isValidPrice = true;
            decimal? newUnitPrice = null;
            Guid? newPriceDetailId = null;
            string? newPriceName = null;
            bool isValidInventory = true;
            int? availableInventory = null;

            // Check if variant is still active
            if (variants.TryGetValue(item.ItemId, out var variant))
            {
                isVariantActive = variant.IsActive;
            }

            // Check price validity
            if (item.InStockProductPriceDetailId.HasValue)
            {
                var currentPriceDetail = await _queryRepository.GetInStockPriceDetailByIdAsync(
                    item.InStockProductPriceDetailId.Value,
                    cancellationToken);

                // Try to get price from priceDetailsMap if currentPriceDetail is null
                if (currentPriceDetail == null && priceDetailsMap.TryGetValue(item.InStockProductPriceDetailId.Value, out var priceFromMap))
                {
                    // Price detail was deleted but we have the cached price
                    unitPrice = priceFromMap;
                    totalPrice = priceFromMap * item.Quantity;
                    isValidPrice = false;
                    
                    // Try to get the highest priority active price
                    var activePriceDetails = await _queryRepository.GetAllActivePriceDetailsByVariantIdAsync(
                        item.ItemId,
                        cancellationToken);

                    if (activePriceDetails.Any())
                    {
                        var highestPriorityDetail = activePriceDetails.First();
                        newUnitPrice = highestPriorityDetail.UnitPrice;
                        newPriceDetailId = highestPriorityDetail.Id;
                        
                        // Get price name
                        var priceInfo = await _queryRepository.GetInStockPriceByIdAsync(
                            highestPriorityDetail.InStockPriceId,
                            cancellationToken);
                        if (priceInfo != null)
                        {
                            newPriceName = priceInfo.Name;
                        }
                    }
                }
                else if (currentPriceDetail != null)
                {
                    // Always set unit price and total price from current price detail
                    unitPrice = currentPriceDetail.UnitPrice;
                    totalPrice = currentPriceDetail.UnitPrice * item.Quantity;

                    if (currentPriceDetail.IsActive)
                    {
                        // Get all active price details for this variant to check if current price is highest priority
                        var activePriceDetails = await _queryRepository.GetAllActivePriceDetailsByVariantIdAsync(
                            item.ItemId,
                            cancellationToken);

                        if (activePriceDetails.Any())
                        {
                            var highestPriorityDetail = activePriceDetails.First(); // Already ordered by priority in query

                            if (currentPriceDetail.Id != highestPriorityDetail.Id)
                            {
                                // Price has changed to a higher priority one
                                isValidPrice = false;
                                newUnitPrice = highestPriorityDetail.UnitPrice;
                                newPriceDetailId = highestPriorityDetail.Id;
                                
                                // Get price name
                                var priceInfo = await _queryRepository.GetInStockPriceByIdAsync(
                                    highestPriorityDetail.InStockPriceId,
                                    cancellationToken);
                                if (priceInfo != null)
                                {
                                    newPriceName = priceInfo.Name;
                                }
                            }
                        }
                        else
                        {
                            // No active prices available
                            isValidPrice = false;
                        }
                    }
                    else
                    {
                        // Current price detail is not active
                        isValidPrice = false;
                        
                        // Try to get the highest priority active price
                        var activePriceDetails = await _queryRepository.GetAllActivePriceDetailsByVariantIdAsync(
                            item.ItemId,
                            cancellationToken);

                        if (activePriceDetails.Any())
                        {
                            var highestPriorityDetail = activePriceDetails.First();
                            newUnitPrice = highestPriorityDetail.UnitPrice;
                            newPriceDetailId = highestPriorityDetail.Id;
                            
                            // Get price name
                            var priceInfo = await _queryRepository.GetInStockPriceByIdAsync(
                                highestPriorityDetail.InStockPriceId,
                                cancellationToken);
                            if (priceInfo != null)
                            {
                                newPriceName = priceInfo.Name;
                            }
                        }
                    }
                }
                else
                {
                    // Price detail not found anywhere
                    isValidPrice = false;
                    
                    // Try to get the highest priority active price
                    var activePriceDetails = await _queryRepository.GetAllActivePriceDetailsByVariantIdAsync(
                        item.ItemId,
                        cancellationToken);

                    if (activePriceDetails.Any())
                    {
                        var highestPriorityDetail = activePriceDetails.First();
                        newUnitPrice = highestPriorityDetail.UnitPrice;
                        newPriceDetailId = highestPriorityDetail.Id;
                        
                        // Get price name
                        var priceInfo = await _queryRepository.GetInStockPriceByIdAsync(
                            highestPriorityDetail.InStockPriceId,
                            cancellationToken);
                        if (priceInfo != null)
                        {
                            newPriceName = priceInfo.Name;
                        }
                    }
                }

                // Check inventory validity
                if (inventoriesMap.TryGetValue(item.ItemId, out var inventory))
                {
                    if (item.Quantity > inventory.TotalQuantity)
                    {
                        isValidInventory = false;
                        availableInventory = inventory.TotalQuantity;
                    }
                }
                else
                {
                    // No inventory found
                    isValidInventory = false;
                    availableInventory = 0;
                }
            }

            var cartItemDto = new CartItemDto(
                item.Id.Value,
                item.ItemId,
                unitPrice,
                item.InStockProductPriceDetailId,
                item.Quantity,
                totalPrice,
                productDetailsMap.TryGetValue(item.ItemId, out var details) ? details : null,
                isVariantActive,
                isValidPrice,
                newUnitPrice,
                newPriceDetailId,
                newPriceName,
                isValidInventory,
                availableInventory);

            cartItems.Add(cartItemDto);
        }

        var cartDto = new CartDto(
            cart.Id.Value,
            cart.UserId,
            cart.CartType,
            cart.TotalItem,
            cartItems);

        return Result.Success(cartDto);
    }
}


