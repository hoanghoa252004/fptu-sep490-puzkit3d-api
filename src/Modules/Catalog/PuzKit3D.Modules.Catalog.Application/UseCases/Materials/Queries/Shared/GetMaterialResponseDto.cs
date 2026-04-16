namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.Shared;

public sealed record GetMaterialResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    decimal FactorPercentage,
    decimal BasePrice);
