using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.Services;
using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Queries.GetCart;

internal sealed class GetInStockCartQueryHandler : IQueryHandler<GetInStockCartQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;
    private readonly IAssetUrlService _assetUrlService;

    public GetInStockCartQueryHandler(
        ICartRepository cartRepository,
        ICartQueryRepository queryRepository,
        IAssetUrlService assetUrlService)
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
        
        var productDetailsMap = variants.ToDictionary(
            kvp => kvp.Key,
            kvp => new ProductDetailsDto(
                $"{kvp.Value.Color} - {kvp.Value.AssembledLengthMm}x{kvp.Value.AssembledWidthMm}x{kvp.Value.AssembledHeightMm}mm",
                kvp.Value.Sku,
                kvp.Value.Color,
                kvp.Value.AssembledLengthMm,
                kvp.Value.AssembledWidthMm,
                kvp.Value.AssembledHeightMm,
                products.TryGetValue(kvp.Value.InStockProductId, out var product) 
                    ? _assetUrlService.BuildAssetUrl(product.ThumbnailUrl)
                    : null,
                kvp.Value.IsActive));

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

        var cartDto = new CartDto(
            cart.Id.Value,
            cart.UserId,
            cart.CartType,
            cart.TotalItem,
            cart.Items.Select(i =>
            {
                decimal? unitPrice = null;
                decimal? totalPrice = null;

                if (i.InStockProductPriceDetailId.HasValue &&
                    priceDetailsMap.TryGetValue(i.InStockProductPriceDetailId.Value, out var price))
                {
                    unitPrice = price;
                    totalPrice = price * i.Quantity;
                }

                return new CartItemDto(
                    i.Id.Value,
                    i.ItemId,
                    unitPrice,
                    i.InStockProductPriceDetailId,
                    i.Quantity,
                    totalPrice,
                    productDetailsMap.TryGetValue(i.ItemId, out var details) ? details : null);
            }).ToList());

        return Result.Success(cartDto);
    }
}


