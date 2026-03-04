using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.UpdateItem;

public sealed record UpdatePartnerCartItemCommand(
    Guid ItemId,
    int Quantity) : ICommand;
