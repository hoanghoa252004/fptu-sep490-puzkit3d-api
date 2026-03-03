using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.RemoveCartItem;

public sealed record RemoveCartItemCommand(
    string ItemType,
    Guid ItemId) : ICommand;
