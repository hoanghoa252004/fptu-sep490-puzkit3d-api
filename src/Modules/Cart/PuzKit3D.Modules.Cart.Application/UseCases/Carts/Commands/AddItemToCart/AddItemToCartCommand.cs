using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.AddItemToCart;

public sealed record AddItemToCartCommand(
    Guid UserId,
    Guid CartTypeId,
    Guid ItemId,
    decimal? UnitPrice,
    Guid? InStockProductPriceDetailId,
    int Quantity) : ICommand;
