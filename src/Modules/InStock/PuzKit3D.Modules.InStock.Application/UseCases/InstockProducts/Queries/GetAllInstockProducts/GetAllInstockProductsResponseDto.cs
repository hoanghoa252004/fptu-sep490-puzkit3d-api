namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetAllInstockProducts;

public sealed record GetAllInstockProductsResponseDto(
    Guid Id,
    string Code,
    string Slug,
    string Name,
    int TotalPieceCount,
    string DifficultLevel,
    int EstimatedBuildTime,
    string ThumbnailUrl,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record GetAllInstockProductsPublicResponseDto(
    Guid Id,
    string Code,
    string Slug,
    string Name,
    int TotalPieceCount,
    string DifficultLevel,
    int EstimatedBuildTime,
    string ThumbnailUrl,
    string? Description);
