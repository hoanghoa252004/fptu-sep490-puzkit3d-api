namespace PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;

public sealed record GetFormulaResponseDto(
    Guid Id,
    string Code,
    string Expression,
    string? Description);

public sealed record GetFormulaDetailedResponseDto(
    Guid Id,
    string Code,
    string Expression,
    string? Description,
    DateTime UpdatedAt);
