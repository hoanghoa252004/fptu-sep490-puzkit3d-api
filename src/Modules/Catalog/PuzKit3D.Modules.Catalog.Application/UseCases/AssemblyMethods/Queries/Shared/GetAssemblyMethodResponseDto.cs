namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.Shared;

public sealed record GetAssemblyMethodResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    decimal FactorPercentage);
