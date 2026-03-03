using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;

internal sealed class GetUserCartQueryHandler : IQueryHandler<GetUserCartQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;

    public GetUserCartQueryHandler(
        ICartRepository cartRepository,
        ICartQueryRepository queryRepository)
    {
        _cartRepository = cartRepository;
        _queryRepository = queryRepository;
    }

    public async Task<ResultT<CartDto>> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            request.UserId,
            request.CartType.ToUpper(),
            cancellationToken);

        if (cart == null)
            return Result.Failure<CartDto>(CartError.CartNotFound());

        var itemIds = cart.Items.Select(i => i.ItemId).ToList();
        
        // Get product details based on cart type
        Dictionary<Guid, ProductDetailsDto> productDetailsMap;
        
        if (cart.CartType == "INSTOCK")
        {
            var variants = await _queryRepository.GetInStockProductVariantsByIdsAsync(itemIds, cancellationToken);
            productDetailsMap = variants.ToDictionary(
                kvp => kvp.Key,
                kvp => new ProductDetailsDto(
                    $"{kvp.Value.Color} - {kvp.Value.Size}",
                    kvp.Value.Sku,
                    kvp.Value.Color,
                    kvp.Value.Size,
                    null,
                    kvp.Value.IsActive));
        }
        else // PARTNER
        {
            var products = await _queryRepository.GetPartnerProductsByIdsAsync(itemIds, cancellationToken);
            productDetailsMap = products.ToDictionary(
                kvp => kvp.Key,
                kvp => new ProductDetailsDto(
                    kvp.Value.Name,
                    kvp.Value.PartnerProductSku,
                    null,
                    null,
                    kvp.Value.ThumbnailUrl,
                    kvp.Value.IsActive));
        }

        var cartDto = new CartDto(
            cart.Id.Value,
            cart.UserId,
            cart.CartType,
            cart.TotalItem,
            cart.Items.Select(i => new CartItemDto(
                i.Id.Value,
                i.ItemId,
                i.UnitPrice?.Amount,
                i.InStockProductPriceDetailId,
                i.Quantity,
                i.TotalPrice?.Amount,
                productDetailsMap.TryGetValue(i.ItemId, out var details) ? details : null)).ToList());

        return Result.Success(cartDto);
    }
}

