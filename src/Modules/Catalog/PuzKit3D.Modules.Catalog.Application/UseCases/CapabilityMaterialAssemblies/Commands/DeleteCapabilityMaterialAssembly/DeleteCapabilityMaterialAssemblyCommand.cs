using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.DeleteCapabilityMaterialAssembly;

public sealed record DeleteCapabilityMaterialAssemblyCommand(
    Guid AssemblyMethodId,
    Guid CapabilityMaterialAssemblyId) : ICommand;
