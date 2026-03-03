using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public static class CartError
{
    public static Error InvalidUserId() => 
        Error.Validation("Cart.InvalidUserId", "User ID cannot be empty");

    public static Error InvalidCartTypeId() => 
        Error.Validation("Cart.InvalidCartTypeId", "Cart type ID cannot be empty");

    public static Error CartNotFound() => 
        Error.NotFound("Cart.NotFound", "Cart not found");

    public static Error CartItemNotFound() => 
        Error.NotFound("Cart.CartItemNotFound", "Cart item not found");

    public static Error InvalidQuantity() => 
        Error.Validation("Cart.InvalidQuantity", "Quantity must be greater than 0");

    public static Error InvalidItemId() => 
        Error.Validation("Cart.InvalidItemId", "Item ID cannot be empty");

    public static Error InvalidPrice() => 
        Error.Validation("Cart.InvalidPrice", "Price must be greater than or equal to 0");

    public static Error DuplicateCartItem() => 
        Error.Conflict("Cart.DuplicateCartItem", "Item already exists in cart");

    public static Error MaxItemsExceeded(int maxItems) => 
        Error.Validation("Cart.MaxItemsExceeded", $"Cannot add more than {maxItems} items to cart");
}
