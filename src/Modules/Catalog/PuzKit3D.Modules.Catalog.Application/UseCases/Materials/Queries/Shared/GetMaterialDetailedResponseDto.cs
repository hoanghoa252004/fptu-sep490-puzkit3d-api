namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.Shared;

public sealed record GetMaterialDetailedResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
