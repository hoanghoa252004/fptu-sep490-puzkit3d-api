namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductBySlug;

public sealed record GetInstockProductBySlugResponseDto(
    Guid Id,
    string Code,
    string Slug,
    string Name,
    int TotalPieceCount,
    string DifficultLevel,
    int EstimatedBuildTime,
    string ThumbnailUrl,
    string PreviewAsset,
    string? Description,
    Guid TopicId,
    Guid AssemblyMethodId,
    Guid CapabilityId,
    Guid MaterialId,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

