namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetInstockProductVariantById;

public sealed record GetInstockProductVariantByIdResponseDto(
    Guid Id,
    Guid ProductId,
    string Sku,
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    List<string> PreviewImages,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);


