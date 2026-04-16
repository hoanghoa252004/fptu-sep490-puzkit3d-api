using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;

public sealed class TopicMaterialCapabilityId : StronglyTypedId<Guid>
{
    private TopicMaterialCapabilityId(Guid value) : base(value) { }

    public static TopicMaterialCapabilityId Create() => new(Guid.NewGuid());

    public static TopicMaterialCapabilityId From(Guid value) => new(value);
}