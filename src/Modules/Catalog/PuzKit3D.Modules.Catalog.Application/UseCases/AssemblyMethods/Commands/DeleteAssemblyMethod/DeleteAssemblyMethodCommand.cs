using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.DeleteAssemblyMethod;

public sealed record DeleteAssemblyMethodCommand(Guid Id) : ICommand;

