using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;

public class CapabilityMaterialAssembly : AggregateRoot<CapabilityMaterialAssemblyId>
{
    public CapabilityId CapabilityId { get; private set; } = null!;
    public MaterialId MaterialId { get; private set; } = null!;
    public AssemblyMethodId AssemblyId { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private CapabilityMaterialAssembly(
        CapabilityMaterialAssemblyId id,
        CapabilityId capabilityId,
        MaterialId materialId,
        AssemblyMethodId assemblyId,
        bool isActive) : base(id)
    {
        CapabilityId = capabilityId;
        MaterialId = materialId;
        AssemblyId = assemblyId;
        IsActive = isActive;
    }

    private CapabilityMaterialAssembly() : base()
    {
    }

    public static CapabilityMaterialAssembly Create(
        CapabilityId capabilityId,
        MaterialId materialId,
        AssemblyMethodId assemblyId,
        bool isActive = false)
    {
        var id = CapabilityMaterialAssemblyId.Create();
        return new CapabilityMaterialAssembly(id, capabilityId, materialId, assemblyId, isActive);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
