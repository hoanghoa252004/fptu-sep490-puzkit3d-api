using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public sealed class CartItem : Entity<CartItemId>
{
    public CartId CartId { get; private set; }
    public Guid ItemId { get; private set; }
    public Guid? InStockProductPriceDetailId { get; private set; }
    public int Quantity { get; private set; }

    private CartItem(
        CartItemId id,
        CartId cartId,
        Guid itemId,
        Guid? inStockProductPriceDetailId,
        int quantity) : base(id)
    {
        CartId = cartId;
        ItemId = itemId;
        InStockProductPriceDetailId = inStockProductPriceDetailId;
        Quantity = quantity;
    }

    private CartItem() : base()
    {
    }

    public static ResultT<CartItem> Create(
        CartId cartId,
        Guid itemId,
        Guid? inStockProductPriceDetailId,
        int quantity)
    {
        if (cartId == null || cartId.Value == Guid.Empty)
            return Result.Failure<CartItem>(CartError.InvalidUserId());

        if (itemId == Guid.Empty)
            return Result.Failure<CartItem>(CartError.InvalidItemId());

        if (quantity <= 0)
            return Result.Failure<CartItem>(CartError.InvalidQuantity());

        var cartItemId = CartItemId.Create();

        var cartItem = new CartItem(
            cartItemId,
            cartId,
            itemId,
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
