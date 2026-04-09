using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;

public sealed class CapabilityDriveId : StronglyTypedId<Guid>
{
    private CapabilityDriveId(Guid value) : base(value) { }

    public static CapabilityDriveId Create() => new(Guid.NewGuid());

    public static CapabilityDriveId From(Guid value) => new(value);
}
