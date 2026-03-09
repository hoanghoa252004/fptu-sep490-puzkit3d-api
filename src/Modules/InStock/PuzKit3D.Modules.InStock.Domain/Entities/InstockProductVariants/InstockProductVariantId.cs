using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;

public sealed class InstockProductVariantId : StronglyTypedId<Guid>
{
    private InstockProductVariantId(Guid value) : base(value) { }

    public static InstockProductVariantId Create() => new(Guid.NewGuid());

    public static InstockProductVariantId From(Guid value) => new(value);
}
