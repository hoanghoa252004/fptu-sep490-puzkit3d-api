namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductBySlug;

public sealed record GetPartnerProductBySlugResponseDto(
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

public sealed record GetPartnerProductBySlugForCustomerResponseDto(
    Guid Id,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    string ThumbnailUrl,
    List<string> PreviewAssets,
    string Slug,
    string? Description
);
