using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.RemoveItem;

public sealed record RemovePartnerCartItemCommand(Guid ItemId) : ICommand;
