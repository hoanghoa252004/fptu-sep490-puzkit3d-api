namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetPartnerById;

public sealed record GetPartnerByIdResponseDto(
    Guid Id,
    Guid ImportServiceConfigId,
    string Name,
    string? Description,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record GetPartnerByIdPublicResponseDto(
    Guid Id,
    Guid ImportServiceConfigId,
    string Name,
    string? Description,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug);
