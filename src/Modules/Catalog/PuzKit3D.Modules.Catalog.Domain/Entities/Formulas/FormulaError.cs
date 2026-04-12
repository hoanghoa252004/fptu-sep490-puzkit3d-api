using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public static class FormulaError
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Formula.NotFound",
        $"Formula with ID '{id}' was not found.");

    public static Error InvalidCode() => Error.Validation(
        "Formula.InvalidCode",
        "Formula code cannot be empty.");

    public static Error InvalidExpression() => Error.Validation(
        "Formula.InvalidExpression",
        "Formula expression cannot be empty.");

    public static Error CodeAlreadyExists(string code) => Error.Conflict(
        "Formula.CodeAlreadyExists",
        $"A formula with code '{code}' already exists.");
}
