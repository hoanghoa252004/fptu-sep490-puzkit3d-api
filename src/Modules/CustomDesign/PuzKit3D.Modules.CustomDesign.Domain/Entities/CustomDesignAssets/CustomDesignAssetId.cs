using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;

public sealed class CustomDesignAssetId : StronglyTypedId<Guid>
{
    private CustomDesignAssetId(Guid value) : base(value) { }

    public static CustomDesignAssetId Create() => new(Guid.NewGuid());

    public static CustomDesignAssetId From(Guid value) => new(value);
}
