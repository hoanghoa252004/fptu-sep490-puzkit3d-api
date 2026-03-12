using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Commands.UpdateInventory;

public sealed record UpdateInventoryCommand(
    Guid ProductId,
    Guid VariantId,
    int Quantity) : ICommand;
