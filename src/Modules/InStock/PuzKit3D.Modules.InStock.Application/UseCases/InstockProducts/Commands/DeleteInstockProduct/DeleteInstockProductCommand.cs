using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.DeleteInstockProduct;

public sealed record DeleteInstockProductCommand(Guid Id) : ICommand;
