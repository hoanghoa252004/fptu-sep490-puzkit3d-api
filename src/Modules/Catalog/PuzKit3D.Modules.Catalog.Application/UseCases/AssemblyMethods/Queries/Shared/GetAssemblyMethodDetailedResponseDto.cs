namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.Shared;

public sealed record GetAssemblyMethodDetailedResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    decimal FactorPercentage,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
