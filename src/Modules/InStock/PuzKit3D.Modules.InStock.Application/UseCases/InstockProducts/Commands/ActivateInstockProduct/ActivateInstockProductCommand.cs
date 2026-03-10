using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.ActivateInstockProduct;

public sealed record ActivateInstockProductCommand(Guid Id) : ICommand;
