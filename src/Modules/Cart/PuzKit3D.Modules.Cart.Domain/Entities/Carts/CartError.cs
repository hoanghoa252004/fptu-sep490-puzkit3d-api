using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Carts;

public static class CartError
{
    public static Error InvalidUserId() => 
        Error.Validation("Cart.InvalidUserId", "User ID cannot be empty");

    public static Error InvalidCartType() => 
        Error.Validation("Cart.InvalidCartType", "Cart type must be either 'INSTOCK' or 'PARTNER'");

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

    public static Error UnauthorizedAccess() => 
        Error.Forbidden("Cart.UnauthorizedAccess", "Only customers can add items to cart");

    public static Error ItemNotFound() => 
        Error.NotFound("Cart.ItemNotFound", "Item not found");

    public static Error ItemNotActive() => 
        Error.Validation("Cart.ItemNotActive", "Item is not active and cannot be added to cart");

    public static Error InsufficientStock(int available) => 
        Error.Validation("Cart.InsufficientStock", $"Insufficient stock.");

    public static Error InvalidItemType() => 
        Error.Validation("Cart.InvalidItemType", "Item type must be either 'instock' or 'partner'");

    public static Error PriceDetailNotFound() => 
        Error.NotFound("Cart.PriceDetailNotFound", "Price detail not found");

    public static Error PriceDetailNotActive() => 
        Error.Validation("Cart.PriceDetailNotActive", "Price detail is not active");

    public static Error InvalidPriceOfVariant() =>
        Error.Validation("Cart.InvalidPriceOfVariant", "Price detail is not of variant");

    public static Error PriceChanged(decimal newUnitPrice) =>
        Error.Conflict("Cart.PriceChanged", $"The price for this item has changed. New price: {newUnitPrice}");

    public static Error InsufficientInventory(int availableQuantity) =>
        Error.Conflict("Cart.InsufficientInventory", $"Insufficient inventory. Available quantity: {availableQuantity}");

    public static Error PriceNotHighestPriority() =>
        Error.Validation("Cart.PriceNotHighestPriority", "The selected price detail is not the highest priority active price for this variant");

    public static Error ItemPriceMismatch() =>
        Error.Conflict("Cart.ItemPriceMismatch", "The prices are different. Please update the cart item instead");
}
