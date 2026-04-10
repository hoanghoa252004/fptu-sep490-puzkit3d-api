using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Domain.Results;

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

    public static ResultT<CapabilityMaterialAssembly> Create(
        CapabilityId capabilityId,
        MaterialId materialId,
        AssemblyMethodId assemblyId,
        bool isActive = false)
    {
        var id = CapabilityMaterialAssemblyId.Create();
        var result = new CapabilityMaterialAssembly(
            id, 
            capabilityId, 
            materialId, 
            assemblyId, 
            isActive);
        return Result.Success(result);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
