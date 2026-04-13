using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.CreateCapabilityMaterialAssembly;

public sealed record CreateCapabilityMaterialAssemblyCommand(
    Guid AssemblyMethodId,
    Guid CapabilityId,
    Guid MaterialId,
    bool IsActive = false) : ICommandT<object>;
