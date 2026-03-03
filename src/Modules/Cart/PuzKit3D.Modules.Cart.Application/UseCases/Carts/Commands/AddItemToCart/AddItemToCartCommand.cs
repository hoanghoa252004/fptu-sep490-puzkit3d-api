using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.AddItemToCart;

public sealed record AddItemToCartCommand(
    string ItemType,
    Guid ItemId,
    int? Quantity
) : ICommand;

