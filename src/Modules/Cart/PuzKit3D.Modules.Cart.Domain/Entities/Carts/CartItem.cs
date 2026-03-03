using PuzKit3D.Modules.Cart.Domain.ValueObjects;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public sealed class CartItem : Entity<CartItemId>
{
    public CartId CartId { get; private set; }
    public Guid ItemId { get; private set; }
    public Money? UnitPrice { get; private set; }
    public Guid? InStockProductPriceDetailId { get; private set; }
    public int Quantity { get; private set; }

    public Money TotalPrice => UnitPrice != null ? UnitPrice.Multiply(Quantity) : Money.Zero();

    private CartItem(
        CartItemId id,
        CartId cartId,
        Guid itemId,
        Money? unitPrice,
        Guid? inStockProductPriceDetailId,
        int quantity) : base(id)
    {
        CartId = cartId;
        ItemId = itemId;
        UnitPrice = unitPrice;
        InStockProductPriceDetailId = inStockProductPriceDetailId;
        Quantity = quantity;
    }

    private CartItem() : base()
    {
    }

    public static ResultT<CartItem> Create(
        CartId cartId,
        Guid itemId,
        decimal? unitPrice,
        Guid? inStockProductPriceDetailId,
        int quantity)
    {
        if (cartId == null || cartId.Value == Guid.Empty)
            return Result.Failure<CartItem>(CartError.InvalidCartTypeId());

        if (itemId == Guid.Empty)
            return Result.Failure<CartItem>(CartError.InvalidItemId());

        if (quantity <= 0)
            return Result.Failure<CartItem>(CartError.InvalidQuantity());

        if (unitPrice.HasValue && unitPrice.Value < 0)
            return Result.Failure<CartItem>(CartError.InvalidPrice());

        var cartItemId = CartItemId.Create();
        Money? money = unitPrice.HasValue ? Money.Create(unitPrice.Value) : null;

        var cartItem = new CartItem(
            cartItemId,
            cartId,
            itemId,
            money,
            inStockProductPriceDetailId,
            quantity);

        return Result.Success(cartItem);
    }

    public Result UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(CartError.InvalidQuantity());

        Quantity = quantity;
        return Result.Success();
    }

    public Result UpdatePrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            return Result.Failure(CartError.InvalidPrice());

        UnitPrice = Money.Create(unitPrice);
        return Result.Success();
    }

    public void IncrementQuantity(int amount = 1)
    {
        Quantity += amount;
    }

    public Result DecrementQuantity(int amount = 1)
    {
        if (Quantity - amount < 1)
            return Result.Failure(CartError.InvalidQuantity());

        Quantity -= amount;
        return Result.Success();
    }
}
