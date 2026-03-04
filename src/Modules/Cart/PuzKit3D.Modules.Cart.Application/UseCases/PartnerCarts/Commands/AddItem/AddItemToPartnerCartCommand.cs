using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.AddItem;

public sealed record AddItemToPartnerCartCommand(
    Guid ItemId,
    int? Quantity) : ICommand;
