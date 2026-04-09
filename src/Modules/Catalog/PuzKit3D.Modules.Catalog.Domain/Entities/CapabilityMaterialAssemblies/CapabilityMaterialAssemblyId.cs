using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;

public sealed class CapabilityMaterialAssemblyId : StronglyTypedId<Guid>
{
    private CapabilityMaterialAssemblyId(Guid value) : base(value) { }

    public static CapabilityMaterialAssemblyId Create() => new(Guid.NewGuid());

    public static CapabilityMaterialAssemblyId From(Guid value) => new(value);
}