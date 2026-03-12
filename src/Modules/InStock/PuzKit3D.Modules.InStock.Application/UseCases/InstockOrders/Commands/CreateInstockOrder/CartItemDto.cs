namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateInstockOrder;

public sealed record CartItemDto(
    Guid ItemId,
    Guid PriceDetailId,
    int Quantity);
