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

        var cartItems = cart.Items.ToList();
        var itemIds = cartItems.Select(i => i.ItemId).ToList();

        // Get Partner product details
        var products = await _queryRepository.GetPartnerProductsByIdsAsync(itemIds, cancellationToken);
        var productDetailsMap = products.ToDictionary(
            kvp => kvp.Key,
            kvp => new ProductDetailsDto(
                kvp.Value.Id,
                kvp.Value.Name,
                kvp.Value.Slug,
                kvp.Value.Name,
                null,
                null,
                null,
                null,
                null,
                kvp.Value.ThumbnailUrl,
                kvp.Value.IsActive,
                kvp.Value.PartnerId,
                kvp.Value.ReferencePrice));

        var sortedItems = SortCartItemsByPartnerAndRecent(cartItems, productDetailsMap);

        var cartDto = new CartDto(
            cart.Id.Value,
            cart.UserId,
            cart.CartType,
            cart.TotalItem,
            sortedItems.Select(i => new CartItemDto(
                i.Id.Value,
                i.ItemId,
                null,
                i.InStockProductPriceDetailId,
                i.Quantity,
                null,
                productDetailsMap.TryGetValue(i.ItemId, out var details) ? details : null)).ToList());

        return Result.Success(cartDto);
    }

    private List<CartItem> SortCartItemsByPartnerAndRecent(
        List<CartItem> cartItems,
        Dictionary<Guid, ProductDetailsDto> productDetailsMap)
    {
        if (!cartItems.Any())
            return new List<CartItem>();

        var validItems = cartItems
        .Where(item => productDetailsMap.ContainsKey(item.ItemId))
        .ToList();

        var groupedByPartner = validItems
        .GroupBy(item => productDetailsMap[item.ItemId].PartnerId)
        .Select(group => new
        {
            Items = group.OrderByDescending(item => item.CreatedAt).ToList(),
            MaxAddedAt = group.Max(item => item.CreatedAt)
        })
        .OrderByDescending(g => g.MaxAddedAt)
        .ToList();

        return groupedByPartner
        .SelectMany(g => g.Items)
        .ToList();
    }
}
