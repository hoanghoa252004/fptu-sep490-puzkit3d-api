using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Queries.GetCartItem;

internal sealed class GetPartnerCartItemQueryHandler : IQueryHandler<GetPartnerCartItemQuery, CartItemDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;

    public GetPartnerCartItemQueryHandler(
        ICartRepository cartRepository,
        ICartQueryRepository queryRepository)
    {
        _cartRepository = cartRepository;
        _queryRepository = queryRepository;
    }

    public async Task<ResultT<CartItemDto>> Handle(GetPartnerCartItemQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            request.UserId,
            "PARTNER",
            cancellationToken);

        if (cart == null)
            return Result.Failure<CartItemDto>(CartError.CartNotFound());

        var cartItem = cart.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
        
        if (cartItem == null)
            return Result.Failure<CartItemDto>(CartError.CartItemNotFound());

        ProductDetailsDto? productDetails = null;
        
        var product = await _queryRepository.GetPartnerProductByIdAsync(request.ItemId, cancellationToken);
        if (product != null)
        {
            productDetails = new ProductDetailsDto(
                product.Name,
                product.PartnerProductSku,
                null,
                null,
                product.ThumbnailUrl,
                product.IsActive);
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
