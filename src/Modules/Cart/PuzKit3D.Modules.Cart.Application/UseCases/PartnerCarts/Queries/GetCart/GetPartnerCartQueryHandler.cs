using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Queries.GetCart;

internal sealed class GetPartnerCartQueryHandler : IQueryHandler<GetPartnerCartQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;

    public GetPartnerCartQueryHandler(
        ICartRepository cartRepository,
        ICartQueryRepository queryRepository)
    {
        _cartRepository = cartRepository;
        _queryRepository = queryRepository;
    }

    public async Task<ResultT<CartDto>> Handle(GetPartnerCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            request.UserId,
            "PARTNER",
            cancellationToken);

        if (cart == null)
            return Result.Failure<CartDto>(CartError.CartNotFound());

        var itemIds = cart.Items.Select(i => i.ItemId).ToList();
        
        // Get Partner product details
        var products = await _queryRepository.GetPartnerProductsByIdsAsync(itemIds, cancellationToken);
        var productDetailsMap = products.ToDictionary(
            kvp => kvp.Key,
            kvp => new ProductDetailsDto(
                kvp.Value.Name,
                kvp.Value.PartnerProductSku,
                null,
                null,
                kvp.Value.ThumbnailUrl,
                kvp.Value.IsActive));

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
