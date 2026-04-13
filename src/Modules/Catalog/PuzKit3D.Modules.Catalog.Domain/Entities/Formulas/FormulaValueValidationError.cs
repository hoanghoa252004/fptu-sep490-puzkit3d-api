using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

public static class FormulaValueValidationError
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "FormulaValueValidation.NotFound",
        $"FormulaValueValidation with ID '{id}' was not found.");

    public static Error InvalidRange() => Error.Validation(
        "FormulaValueValidation.InvalidRange",
        "MinValue must be less than or equal to MaxValue.");

    public static Error InvalidOutput() => Error.Validation(
        "FormulaValueValidation.InvalidOutput",
        "Output cannot be empty.");
}
