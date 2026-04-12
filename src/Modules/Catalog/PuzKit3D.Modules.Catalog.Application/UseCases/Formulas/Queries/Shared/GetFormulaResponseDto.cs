namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;

public sealed record GetFormulaResponseDto(
    Guid Id,
    string Code,
    string Expression,
    string? Description,
    bool IsNeedValidation,
    List<FormulaValueValidationDto>? FormulaValueValidations = null);

public sealed record GetFormulaDetailedResponseDto(
    Guid Id,
    string Code,
    string Expression,
    string? Description,
    bool IsNeedValidation,
    DateTime UpdatedAt,
    List<FormulaValueValidationDto>? FormulaValueValidations = null);

public sealed record FormulaValueValidationDto(
    Guid Id,
    decimal MinValue,
    decimal MaxValue,
    string Output);


