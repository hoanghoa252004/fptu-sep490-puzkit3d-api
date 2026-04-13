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
List<string> PreviewAsset,
string? Description,
Guid TopicId,
Guid MaterialId,
List<Guid> AssemblyMethodId,
List<Guid> CapabilityIds,
List<GetInstockProductBySlugDriveDetailDto> Drives,
bool IsActive,
DateTime CreatedAt,
DateTime UpdatedAt);



