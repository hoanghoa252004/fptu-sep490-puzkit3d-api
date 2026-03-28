using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.UpdateItem;

public sealed record UpdateInStockCartItemCommand(
Guid ItemId,
int? Quantity = null,
Guid? InStockProductPriceDetailId = null) : ICommand;
