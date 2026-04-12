namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CalculateFormula;

public sealed record FormulaCalculateResponse(
    decimal RawValue,
    string? ValidationOutput = null);
