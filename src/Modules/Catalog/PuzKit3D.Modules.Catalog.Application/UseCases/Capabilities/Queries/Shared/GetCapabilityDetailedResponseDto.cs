namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.Shared;

public sealed record GetCapabilityDetailedResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    decimal FactorPercentage,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
