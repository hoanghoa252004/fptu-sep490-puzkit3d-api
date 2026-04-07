using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;

public sealed class ProductQuotationId : StronglyTypedId<Guid>
{
    private ProductQuotationId(Guid value) : base(value) { }

    public static ProductQuotationId Create() => new(Guid.NewGuid());

    public static ProductQuotationId From(Guid value) => new(value);
}
