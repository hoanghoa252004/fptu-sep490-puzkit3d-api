using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.RemoveItem;

public sealed record RemoveInStockCartItemCommand(Guid ItemId) : ICommand;
