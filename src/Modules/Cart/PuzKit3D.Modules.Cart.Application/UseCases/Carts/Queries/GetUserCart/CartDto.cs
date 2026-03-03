namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;

public sealed record CartDto(
    Guid Id,
    Guid UserId,
    string CartType,
    int TotalItem,
    List<CartItemDto> Items);

public sealed record CartItemDto(
    Guid Id,
    Guid ItemId,
    decimal? UnitPrice,
    Guid? InStockProductPriceDetailId,
    int Quantity,
    decimal? TotalPrice,
    ProductDetailsDto? ProductDetails);

public sealed record ProductDetailsDto(
    string Name,
    string? Sku,
    string? Color,
    string? Size,
    string? ThumbnailUrl,
    bool IsActive);

