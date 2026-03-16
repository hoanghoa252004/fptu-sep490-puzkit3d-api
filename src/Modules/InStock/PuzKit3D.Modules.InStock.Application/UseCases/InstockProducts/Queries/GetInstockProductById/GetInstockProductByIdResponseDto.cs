namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductById;

public sealed record GetInstockProductByIdResponseDto(
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
    List<Guid> CapabilityIds,
    Guid MaterialId,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
