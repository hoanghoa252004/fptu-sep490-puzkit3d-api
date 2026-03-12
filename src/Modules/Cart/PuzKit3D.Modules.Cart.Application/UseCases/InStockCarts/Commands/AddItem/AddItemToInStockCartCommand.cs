using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.AddItem;

public sealed record AddItemToInStockCartCommand(
    Guid ItemId,
    Guid InStockProductPriceDetailId,
    int? Quantity) : ICommand;
