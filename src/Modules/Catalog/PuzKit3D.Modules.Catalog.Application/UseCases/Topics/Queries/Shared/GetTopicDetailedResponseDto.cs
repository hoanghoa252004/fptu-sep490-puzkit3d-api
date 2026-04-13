namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.Shared;

public sealed record GetTopicDetailedResponseDto(
    Guid Id,
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description,
    decimal FactorPercentage,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
