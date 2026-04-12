namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.Shared;

public sealed record GetCapabilityResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    decimal FactorPercentage);
