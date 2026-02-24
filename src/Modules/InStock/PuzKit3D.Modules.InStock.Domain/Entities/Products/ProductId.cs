using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Products;

public sealed class ProductId : StronglyTypedId<Guid>
{
    private ProductId(Guid value) : base(value) { }

    public static ProductId Create() => new(Guid.NewGuid());

    public static ProductId From(Guid value) => new(value);
}