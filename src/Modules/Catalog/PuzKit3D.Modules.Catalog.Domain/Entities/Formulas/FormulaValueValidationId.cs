using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public sealed class FormulaValueValidationId : StronglyTypedId<Guid>
{
    private FormulaValueValidationId(Guid value) : base(value) { }

    public static FormulaValueValidationId Create() => new(Guid.NewGuid());

    public static FormulaValueValidationId From(Guid value) => new(value);
}
