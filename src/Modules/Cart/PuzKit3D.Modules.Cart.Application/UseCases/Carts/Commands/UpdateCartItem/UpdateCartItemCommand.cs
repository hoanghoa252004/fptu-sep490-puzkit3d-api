using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.UpdateCartItem;

public sealed record UpdateCartItemCommand(
    string ItemType,
    Guid ItemId,
    int Quantity) : ICommand;
