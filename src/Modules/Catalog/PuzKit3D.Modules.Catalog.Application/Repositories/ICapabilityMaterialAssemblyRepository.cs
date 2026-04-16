using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface ICapabilityMaterialAssemblyRepository : IRepositoryBase<CapabilityMaterialAssembly, CapabilityMaterialAssemblyId>
{
    Task<IEnumerable<CapabilityMaterialAssembly>> GetCapabilityMaterialAssembliesByMaterialIdAsync(
        MaterialId materialId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<CapabilityMaterialAssembly>> GetCapabilityMaterialAssembliesByCapabilityIdAsync(
        CapabilityId capabilityId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<CapabilityMaterialAssembly>> GetCapabilityMaterialAssembliesByAssemblyMethodIdAsync(
        AssemblyMethodId assemblyMethodId,
        CancellationToken cancellationToken = default);
}
