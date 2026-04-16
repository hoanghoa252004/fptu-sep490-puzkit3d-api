using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.UpdateCapabilityMaterialAssembly;

public sealed record UpdateCapabilityMaterialAssemblyCommand(
    Guid AssemblyMethodId,
    Guid CapabilityMaterialAssemblyId,
    bool IsActive) : ICommandT<Guid>;
