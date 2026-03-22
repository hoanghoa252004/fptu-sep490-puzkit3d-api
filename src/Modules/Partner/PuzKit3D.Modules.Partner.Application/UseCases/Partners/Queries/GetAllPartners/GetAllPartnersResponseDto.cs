namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetAllPartners;

public sealed record GetAllPartnersResponseDto(
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

public sealed record GetAllPartnersPublicResponseDto(
    Guid Id,
    string Name,
    string? Description,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug);
