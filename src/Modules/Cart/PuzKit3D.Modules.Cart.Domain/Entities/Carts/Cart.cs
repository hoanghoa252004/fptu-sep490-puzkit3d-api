using PuzKit3D.Modules.Cart.Domain.Events.Carts;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public sealed class Cart : AggregateRoot<CartId>
{
    private readonly List<CartItem> _items = new();

    public Guid UserId { get; private set; }
    public string CartType { get; private set; } = string.Empty;
    public int TotalItem { get; private set; }

    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    private Cart(
        CartId id,
        Guid userId,
        string cartType) : base(id)
    {
        UserId = userId;
        CartType = cartType;
        TotalItem = 0;
    }

    private Cart() : base()
    {
    }

    public static ResultT<Cart> Create(Guid userId, string cartType)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Cart>(CartError.InvalidUserId());

        if (string.IsNullOrWhiteSpace(cartType))
            return Result.Failure<Cart>(CartError.InvalidCartType());

        var normalizedCartType = cartType.ToUpper();
        if (normalizedCartType != "INSTOCK" && normalizedCartType != "PARTNER")
            return Result.Failure<Cart>(CartError.InvalidCartType());

        var cartId = CartId.Create();
        var cart = new Cart(cartId, userId, normalizedCartType);

        cart.RaiseDomainEvent(new CartCreatedDomainEvent(cartId.Value, userId, normalizedCartType));

        return Result.Success(cart);
    }

    public Result AddItem(
        Guid itemId,
        decimal? unitPrice,
        Guid? inStockProductPriceDetailId,
        int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ItemId == itemId);
        if (existingItem != null)
        {
            existingItem.IncrementQuantity(quantity);
            TotalItem += quantity;
            
            RaiseDomainEvent(new CartItemQuantityChangedDomainEvent(
                Id.Value,
                existingItem.Id.Value,
                itemId,
                existingItem.Quantity));

            return Result.Success();
        }

        var cartItemResult = CartItem.Create(
            Id,
            itemId,
            unitPrice,
            inStockProductPriceDetailId,
            quantity);

        if (cartItemResult.IsFailure)
            return Result.Failure(cartItemResult.Error);

        _items.Add(cartItemResult.Value);
        TotalItem += quantity;

        RaiseDomainEvent(new CartItemAddedDomainEvent(
            Id.Value,
            cartItemResult.Value.Id.Value,
            itemId,
            quantity,
            unitPrice));

        return Result.Success();
    }

    public Result RemoveItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.ItemId == itemId);
        if (item == null)
            return Result.Failure(CartError.CartItemNotFound());

        TotalItem -= item.Quantity;
        _items.Remove(item);

        RaiseDomainEvent(new CartItemRemovedDomainEvent(
            Id.Value,
            item.Id.Value,
            itemId));

        return Result.Success();
    }

    public Result UpdateItemQuantity(Guid itemId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ItemId == itemId);
        if (item == null)
            return Result.Failure(CartError.CartItemNotFound());

        var oldQuantity = item.Quantity;
        var updateResult = item.UpdateQuantity(quantity);

        if (updateResult.IsFailure)
            return updateResult;

        TotalItem = TotalItem - oldQuantity + quantity;

        RaiseDomainEvent(new CartItemQuantityChangedDomainEvent(
            Id.Value,
            item.Id.Value,
            itemId,
            quantity));

        return Result.Success();
    }

    public Result ClearCart()
    {
        if (_items.Count == 0)
            return Result.Success();

        _items.Clear();
        TotalItem = 0;

        RaiseDomainEvent(new CartClearedDomainEvent(Id.Value, UserId));

        return Result.Success();
    }
}
