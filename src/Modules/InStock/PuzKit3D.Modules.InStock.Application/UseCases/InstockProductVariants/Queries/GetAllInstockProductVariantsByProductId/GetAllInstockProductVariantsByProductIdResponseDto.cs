namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetAllInstockProductVariantsByProductId;

public sealed record GetAllInstockProductVariantsByProductIdResponseDto(
    IEnumerable<VariantDto> Variants);

public sealed record VariantDto(
    Guid Id,
    string Sku,
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    List<string> PreviewImages,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record AnonymousVariantDto(
    Guid Id,
    string Sku,
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    List<string> PreviewImages);


