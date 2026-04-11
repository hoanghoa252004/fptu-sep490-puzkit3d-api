using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;

public static class CapabilityMaterialAssemblyError
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "CapabilityMaterialAssembly.NotFound",
        $"CapabilityMaterialAssembly with ID '{id}' was not found.");
}
