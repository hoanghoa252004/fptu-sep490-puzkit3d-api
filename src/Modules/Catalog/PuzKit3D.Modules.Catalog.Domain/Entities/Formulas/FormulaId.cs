using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public sealed class FormulaId : StronglyTypedId<Guid>
{
    private FormulaId(Guid value) : base(value) { }

    public static FormulaId Create() => new(Guid.NewGuid());

    public static FormulaId From(Guid value) => new(value);
}
