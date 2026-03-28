namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductById;

internal sealed record GetPartnerProductByIdResponseDto(
    Guid Id,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    int Quantity,
    string ThumbnailUrl,
    List<string> PreviewAssets,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

internal sealed record GetPartnerProductByIdForCustomerResponseDto(
    Guid Id,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    string ThumbnailUrl,
    List<string> PreviewAssets,
    string Slug,
    string? Description
);