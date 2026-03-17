namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.Shared;

public sealed record GetTopicDetailedResponseDto(
    Guid Id,
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
