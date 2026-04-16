namespace PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.Shared;

public sealed record GetFormulaValueValidationResponseDto(
    Guid Id,
    Guid FormulaId,
    decimal MinValue,
    decimal MaxValue,
    string Output);

public sealed record GetFormulaValueValidationDetailedResponseDto(
    Guid Id,
    Guid FormulaId,
    decimal MinValue,
    decimal MaxValue,
    string Output,
    DateTime UpdatedAt);
