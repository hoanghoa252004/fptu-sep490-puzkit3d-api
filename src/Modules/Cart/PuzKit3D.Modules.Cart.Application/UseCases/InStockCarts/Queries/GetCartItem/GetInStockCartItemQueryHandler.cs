using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Queries.GetCartItem;

internal sealed class GetInStockCartItemQueryHandler : IQueryHandler<GetInStockCartItemQuery, CartItemDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;

    public GetInStockCartItemQueryHandler(
        ICartRepository cartRepository,
        ICartQueryRepository queryRepository)
    {
        _cartRepository = cartRepository;
        _queryRepository = queryRepository;
    }

    public async Task<ResultT<CartItemDto>> Handle(GetInStockCartItemQuery request, CancellationToken cancellationToken)
    {
        // Get cart
        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            request.UserId,
            "INSTOCK",
            cancellationToken);

        if (cart == null)
            return Result.Failure<CartItemDto>(CartError.CartNotFound());

        // Find item in cart
        var cartItem = cart.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
        
        if (cartItem == null)
            return Result.Failure<CartItemDto>(CartError.CartItemNotFound());

        // Get product details
        ProductDetailsDto? productDetails = null;
        
        var variant = await _queryRepository.GetInStockProductVariantByIdAsync(request.ItemId, cancellationToken);
        if (variant != null)
        {
            productDetails = new ProductDetailsDto(
                $"{variant.Color} - {variant.Size}",
                variant.Sku,
                variant.Color,
                variant.Size,
                null,
                variant.IsActive);
        }

        var cartItemDto = new CartItemDto(
            cartItem.Id.Value,
            cartItem.ItemId,
            cartItem.UnitPrice?.Amount,
            cartItem.InStockProductPriceDetailId,
            cartItem.Quantity,
            cartItem.TotalPrice?.Amount,
            productDetails);

        return Result.Success(cartItemDto);
    }
}
