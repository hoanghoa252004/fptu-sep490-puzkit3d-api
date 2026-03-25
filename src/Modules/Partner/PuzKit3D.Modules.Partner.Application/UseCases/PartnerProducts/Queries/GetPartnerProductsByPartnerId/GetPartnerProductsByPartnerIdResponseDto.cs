namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductsByPartnerId;

public sealed record GetPartnerProductsByPartnerIdResponseDto(
    Guid Id,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    string ThumbnailUrl,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record GetPartnerProductsByPartnerIdPublicResponseDto(
    Guid Id,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    string ThumbnailUrl,
    string Slug,
    string? Description);
