using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Commands.DeleteInventory;

public sealed record DeleteInventoryCommand(
    Guid ProductId,
    Guid VariantId) : ICommand;
