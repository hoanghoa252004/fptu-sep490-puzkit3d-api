using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.Partners;

public sealed class PartnerId : StronglyTypedId<Guid>
{
    private PartnerId(Guid value) : base(value) { }

    public static PartnerId Create() => new(Guid.NewGuid());

    public static PartnerId From(Guid value) => new(value);
}
