using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.RemoveItemFromCart;

public sealed record RemoveItemFromCartCommand(
    Guid UserId,
    Guid CartTypeId,
    Guid ItemId) : ICommand;
