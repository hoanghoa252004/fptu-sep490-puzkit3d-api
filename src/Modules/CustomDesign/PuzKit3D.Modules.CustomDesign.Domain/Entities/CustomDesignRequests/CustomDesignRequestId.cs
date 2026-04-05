using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

public sealed class CustomDesignRequestId : StronglyTypedId<Guid>
{
    private CustomDesignRequestId(Guid value) : base(value) { }

    public static CustomDesignRequestId Create() => new(Guid.NewGuid());

    public static CustomDesignRequestId From(Guid value) => new(value);
}
