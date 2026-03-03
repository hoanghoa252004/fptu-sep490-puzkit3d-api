namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;

public sealed record CartDto(
    Guid Id,
    Guid UserId,
    Guid CartTypeId,
    int TotalItem,
    List<CartItemDto> Items);

public sealed record CartItemDto(
    Guid Id,
    Guid ItemId,
    decimal? UnitPrice,
    Guid? InStockProductPriceDetailId,
    int Quantity,
    decimal? TotalPrice);
