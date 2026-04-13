using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public static class FormulaError
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Formula.NotFound",
        $"Formula with ID '{id}' was not found.");

    public static Error InvalidExpression() => Error.Validation(
        "Formula.InvalidExpression",
        "Formula expression cannot be empty.");

    public static Error CannotCreateFormula() => Error.Conflict(
        "Formula.CannotCreateFormula",
        "Creating formulas is not allowed. Formulas are predefined by the system.");

    public static Error CannotDeleteFormula() => Error.Conflict(
        "Formula.CannotDeleteFormula",
        "Deleting formulas is not allowed. Formulas are predefined by the system.");
}


