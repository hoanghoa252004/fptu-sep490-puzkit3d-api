using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Commands.CreateInventory;

public sealed record CreateInventoryCommand(
    Guid ProductId,
    Guid VariantId,
    int Quantity) : ICommand;
