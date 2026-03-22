namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetPartnerBySlug;

public sealed record GetPartnerBySlugResponseDto(
    Guid Id,
    string Name,
    string? Description,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record GetPartnerBySlugPublicResponseDto(
    Guid Id,
    string Name,
    string? Description,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug);
