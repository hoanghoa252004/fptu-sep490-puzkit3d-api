namespace PuzKit3D.Modules.Cart.Application.SharedResponseDto;

public sealed record ProductDetailsDto(
Guid ProductId,
string ProductName,
string Slug,
string VariantName,
string? Sku,
string? Color,
int? AssembledLengthMm,
int? AssembledWidthMm,
int? AssembledHeightMm,
string? ThumbnailUrl,
bool IsActive,
Guid? PartnerId,
decimal? ReferencePrice);
